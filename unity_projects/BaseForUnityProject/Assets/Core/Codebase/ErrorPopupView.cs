using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Codebase.Scripts.UICommon
{
    public class ErrorPopupView : MonoBehaviour
    {
        [SerializeField] private TMP_Text errorPopupTitle;
        [SerializeField] private TMP_Text errorText;
        [SerializeField] private Button close;
        private Action _okClick;

        private void Awake()
        {
            close.onClick.AddListener(DestroyPopup);
        }

        public void Initialize(string errorMessage, string popupTitle, Action okClick = null)
        {
            _okClick = okClick;
            errorPopupTitle.text = popupTitle;
            errorText.text = errorMessage;
        }

        private void DestroyPopup()
        {
            Destroy(gameObject);
            _okClick?.Invoke();
        }
    }
}