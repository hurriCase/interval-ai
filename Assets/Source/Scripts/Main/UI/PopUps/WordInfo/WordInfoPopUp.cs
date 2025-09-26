using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using CustomUtils.Runtime.Extensions;
using CustomUtils.Runtime.Extensions.Observables;
using R3;
using Source.Scripts.Core.Localization.LocalizationTypes;
using Source.Scripts.Core.Others;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.Base;
using Source.Scripts.Main.UI.PopUps.WordInfo.Behaviours;
using Source.Scripts.Main.UI.PopUps.WordInfo.Behaviours.AdditionalItems;
using Source.Scripts.Main.UI.PopUps.WordPractice;
using Source.Scripts.UI.Components.Accordion;
using Source.Scripts.UI.Components.Button;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;
using VContainer;
using ZLinq;

namespace Source.Scripts.Main.UI.PopUps.WordInfo
{
    internal sealed class WordInfoPopUp : PopUpBase
    {
        [SerializeField] private ButtonComponent _startLearningButton;

        [SerializeField] private WordInfoCardBehaviour _wordInfoCardBehaviour;

        [SerializeField] private AnotherWordItem _anotherWordItem;
        [SerializeField] private TranslatableInfoItem _translatableInfoItem;
        [SerializeField] private TranslatableInfoItemWithNote _translatableInfoItemWithNote;

        [SerializeField] private EnumArray<AdditionalInfoType, AccordionComponent> _sections = new(EnumMode.SkipFirst);

        private UIPool<Translation, TranslatableInfoItem> _exampleInfoPool;
        private UIPool<Translation, TranslatableInfoItem> _translationVariantsPool;
        private UIPool<TranslationSet, AnotherWordItem> _synonymPool;
        private UIPool<AnnotatedTranslation, TranslatableInfoItemWithNote> _grammarPool;

        private WordEntry _currentWordEntry;

        private ICurrentWordsService _currentWordsService;
        private IWindowsController _windowsController;
        private IObjectResolver _objectResolver;

        [Inject]
        internal void Inject(
            ICurrentWordsService currentWordsService,
            IWindowsController windowsController,
            IObjectResolver objectResolver)
        {
            _currentWordsService = currentWordsService;
            _windowsController = windowsController;
            _objectResolver = objectResolver;
        }

        internal override void Init()
        {
            _wordInfoCardBehaviour.Init();

            _startLearningButton.OnClickAsObservable()
                .SubscribeUntilDestroy(this, static self => self.StartPracticeForCurrentWord());

            CreatePools();
        }

        internal void SetParameters(WordEntry wordEntry)
        {
            _wordInfoCardBehaviour.UpdateView(wordEntry);

            _currentWordEntry = wordEntry;

            _startLearningButton.SetActive(_currentWordEntry.LearningState == LearningState.Default);

            CreateInfoSections();
        }

        private void CreatePools()
        {
            _exampleInfoPool = CreatePool<Translation, TranslatableInfoItem>(
                _translatableInfoItem,
                AdditionalInfoType.Example);

            _translationVariantsPool = CreatePool<Translation, TranslatableInfoItem>(
                _translatableInfoItem,
                AdditionalInfoType.TranslationVariant);

            _synonymPool = CreatePool<TranslationSet, AnotherWordItem>(
                _anotherWordItem,
                AdditionalInfoType.Synonym);

            _grammarPool = CreatePool<AnnotatedTranslation, TranslatableInfoItemWithNote>(
                _translatableInfoItemWithNote,
                AdditionalInfoType.Grammar);
        }

        private void CreateInfoSections()
        {
            CreateInfoSection(_exampleInfoPool, _currentWordEntry.Examples, AdditionalInfoType.Example);
            CreateInfoSection(_translationVariantsPool, _currentWordEntry.TranslationVariants,
                AdditionalInfoType.TranslationVariant);
            CreateInfoSection(_synonymPool, _currentWordEntry.Synonyms, AdditionalInfoType.Synonym);
            CreateInfoSection(_grammarPool, _currentWordEntry.Grammar, AdditionalInfoType.Grammar);
        }

        private UIPool<TTranslation, TItem> CreatePool<TTranslation, TItem>(
            TItem additionalInfoItem,
            AdditionalInfoType type)
            where TItem : MonoBehaviour, IAdditionalInfoItemBase<TTranslation>
            where TTranslation : ITranslation
        {
            var section = _sections[type];
            var poolEvents = new UIPoolEvents<TTranslation, TItem>(
                (_, item) => item.Init(),
                (translation, item) => item.UpdateView(translation));

            var pool = new UIPool<TTranslation, TItem>(
                additionalInfoItem,
                section.HiddenContentContainer,
                _objectResolver,
                poolEvents);
            return pool;
        }

        private void CreateInfoSection<TItem, TTranslation>(
            UIPool<TTranslation, TItem> itemsPool,
            List<TTranslation> translations,
            AdditionalInfoType type)
            where TItem : MonoBehaviour, IAdditionalInfoItemBase<TTranslation>
            where TTranslation : ITranslation
        {
            var section = _sections[type];
            if (translations is null || translations.Count == 0)
            {
                section.SetActive(false);
                return;
            }

            var validTranslations = translations.AsValueEnumerable()
                .Where(translation => translation.IsValid);

            itemsPool.EnsureCount(validTranslations.ToList());

            section.SetActive(true);
        }

        private void StartPracticeForCurrentWord()
        {
            _currentWordsService.SetCurrentWord(PracticeState.NewWords, _currentWordEntry);
            _windowsController.OpenPopUp<WordPracticePopUp>();
        }
    }
}