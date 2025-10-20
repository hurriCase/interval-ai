using System.Collections.Generic;
using CustomUtils.Runtime.Extensions;
using Source.Scripts.Core.Others.UIPools;
using Source.Scripts.Core.Repositories.Words.Base;
using Source.Scripts.Main.UI.PopUps.WordInfo.Behaviours.AdditionalItems;
using Source.Scripts.UI.Components.Accordion;
using UnityEngine;
using VContainer;
using ZLinq;

namespace Source.Scripts.Main.UI.PopUps.WordInfo.Behaviours
{
    internal class AdditionalInfoContainer<TTranslation> : MonoBehaviour where TTranslation : ITranslation
    {
        [SerializeField] private AdditionalInfoItemBase<TTranslation> _infoItem;
        [SerializeField] private AccordionComponent _accordionComponent;

        private UIPoolWithData<TTranslation, AdditionalInfoItemBase<TTranslation>> _itemsPoolWithData;

        private IObjectResolver _objectResolver;

        [Inject]
        internal void Inject(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }

        internal void Init()
        {
            var poolEvents = new UIPoolEvents<TTranslation, AdditionalInfoItemBase<TTranslation>>(
                onCreated: static (_, item) => item.Init(),
                onActivated: static (translation, item) => item.UpdateView(translation));

            _itemsPoolWithData = new UIPoolWithData<TTranslation, AdditionalInfoItemBase<TTranslation>>(
                _infoItem, _accordionComponent.HiddenContentContainer, poolEvents, _objectResolver);
        }

        internal void UpdateView(List<TTranslation> translations)
        {
            if (translations is null || translations.Count == 0)
            {
                _accordionComponent.SetActive(false);
                return;
            }

            using var validTranslations = translations
                .Where(static translation => translation.IsValid)
                .ToArrayPool();

            var validTranslationsSpan = validTranslations.Span;
            if (validTranslationsSpan.Length == 0)
            {
                _accordionComponent.SetActive(false);
                return;
            }

            _itemsPoolWithData.EnsureCount(validTranslationsSpan);

            _accordionComponent.SetActive(true);
        }
    }
}