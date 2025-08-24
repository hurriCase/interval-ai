using System.Collections.Generic;
using CustomUtils.Runtime.CustomTypes.Collections;
using R3;
using R3.Triggers;
using Source.Scripts.Core.Others;
using Source.Scripts.Core.Repositories.Words.Word;
using Source.Scripts.Main.UI.PopUps.WordInfo.Behaviours;
using Source.Scripts.Main.UI.PopUps.WordInfo.Behaviours.AdditionalItems;
using Source.Scripts.UI.Components;
using Source.Scripts.UI.Windows.Base;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using ZLinq;

namespace Source.Scripts.Main.UI.PopUps.WordInfo
{
    internal sealed class WordInfoPopUp : PopUpBase
    {
        [SerializeField] private ButtonComponent _editButton;
        [SerializeField] private ButtonComponent _startLearningButton;

        [SerializeField] private RectTransform _cardContainer;
        [SerializeField] private RectTransform _cardContent;

        [SerializeField] private WordInfoCardBehaviour _wordInfoCardBehaviour;

        [SerializeField] private TranslatableInfoItem _translatableInfoItem;
        [SerializeField] private AnotherWordItem _anotherWordItem;
        [SerializeField] private TranslatableInfoItemWithNote _translatableInfoItemWithNote;
        [SerializeField] private AspectRatioFitter _spacing;
        [SerializeField] private float _additionalItemSpacingRatio;

        [SerializeField] private EnumArray<AdditionalInfoType, AdditionalSectionItem> _sections
            = new(EnumMode.SkipFirst);

        [Inject] private IObjectResolver _objectResolver;

        private UIPool<TranslatableInfoItem> _exampleInfoPool;
        private UIPool<TranslatableInfoItem> _translationVariantsPool;
        private UIPool<AnotherWordItem> _synonymPool;
        private UIPool<TranslatableInfoItemWithNote> _grammarPool;

        private WordEntry _currentWordEntry;

        internal override void Init()
        {
            _wordInfoCardBehaviour.Init();

            _cardContent.OnRectTransformDimensionsChangeAsObservable()
                .SubscribeAndRegister(this, static self => self.UpdateContainerHeight());

            _exampleInfoPool = CreatePool<TranslatableInfoItem, Translation>(
                _translatableInfoItem,
                AdditionalInfoType.Example);

            _translationVariantsPool = CreatePool<TranslatableInfoItem, Translation>(
                _translatableInfoItem,
                AdditionalInfoType.TranslationVariant);

            _synonymPool = CreatePool<AnotherWordItem, TranslationSet>(
                _anotherWordItem,
                AdditionalInfoType.Synonym);

            _grammarPool = CreatePool<TranslatableInfoItemWithNote, AnnotatedTranslation>(
                _translatableInfoItemWithNote,
                AdditionalInfoType.Grammar);
        }

        private UIPool<TItem> CreatePool<TItem, TTranslation>(TItem additionalInfoItem, AdditionalInfoType type)
            where TItem : AccordionItem, IAdditionalInfoItemBase<TTranslation>
            where TTranslation : ITranslation
        {
            var section = _sections[type];

            section.Accordion.Init();

            var pool = new UIPool<TItem>(
                additionalInfoItem,
                section.Accordion.HiddenContentContainer.RectTransform,
                _spacing,
                _additionalItemSpacingRatio,
                AspectRatioFitter.AspectMode.WidthControlsHeight,
                _objectResolver);

            pool.CreateWithSpaceObservable
                .SubscribeAndRegister(this, section, static (createdItem, self, sectionData) =>
                    self.OnCreateSection<TItem, TTranslation>(createdItem, sectionData));

            return pool;
        }

        private void OnCreateSection<TItem, TTranslation>(
            PrefabData<TItem> createdItem,
            AdditionalSectionItem sectionItem)
            where TItem : AccordionItem, IAdditionalInfoItemBase<TTranslation>
            where TTranslation : ITranslation
        {
            sectionItem.Accordion.HiddenContent.Add(createdItem.Prefab);
            sectionItem.SizeCopier.AddObservedTarget(createdItem.Prefab.RectTransform);
            sectionItem.SizeCopier.AddSourceToSum(createdItem.Prefab.RectTransform);
            if (createdItem.Spacing.TryGetComponent<RectTransform>(out var spacingRect))
                sectionItem.SizeCopier.AddSourceToSum(spacingRect);
        }

        internal void SetParameters(WordEntry wordEntry)
        {
            _wordInfoCardBehaviour.UpdateView(wordEntry);

            _currentWordEntry = wordEntry;

            CreateInfoSections();
        }

        private void UpdateContainerHeight()
        {
            _cardContainer.sizeDelta = new Vector2(_cardContainer.sizeDelta.x, _cardContent.rect.height);

            LayoutRebuilder.ForceRebuildLayoutImmediate(_cardContainer.parent as RectTransform);
        }

        private void CreateInfoSections()
        {
            CreateInfoSection(_exampleInfoPool, _currentWordEntry.Examples, AdditionalInfoType.Example);
            CreateInfoSection(_translationVariantsPool, _currentWordEntry.TranslationVariants, AdditionalInfoType.TranslationVariant);
            CreateInfoSection(_synonymPool, _currentWordEntry.Synonyms, AdditionalInfoType.Synonym);
            CreateInfoSection(_grammarPool, _currentWordEntry.Grammar, AdditionalInfoType.Grammar);
        }

        private void CreateInfoSection<TItem, TTranslation>(
            UIPool<TItem> itemsPool,
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

            var examplesCount = validTranslations.Count();
            itemsPool.EnsureCount(examplesCount);

            var index = 0;
            foreach (var validTranslation in validTranslations)
            {
                itemsPool.PooledItems[index].Init(validTranslation);
                index++;
            }

            section.SetActive(true);
        }
    }
}