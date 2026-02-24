using System;
using BG_Games.Card_Game_Core.Cards.Controllers.Animations;
using BG_Games.Card_Game_Core.Cards.Controllers.Audio;
using BG_Games.Card_Game_Core.Cards.Info;
using BG_Games.Card_Game_Core.Player;
using BG_Games.Card_Game_Core.Systems;
using BG_Games.Card_Game_Core.UI.CardInfoHolders;
using UnityEngine;
using Zenject;

namespace BG_Games.Card_Game_Core.Cards.Controllers
{
    public abstract class Card : MonoBehaviour,ICard,ISupportLockInput,ISupportInputReading
    {
        [SerializeField] protected Collider2D Collider;
        [Header("Audio")]
        [SerializeField] protected CardAudio CardAudio;
        [Space] 
        [SerializeField] protected float _zPosOnTop = -1;
        [SerializeField] protected string _defaulSortingtLayer = "Card";
        [SerializeField] protected string _topSortingLayer = "Over UI";

        private Vector3 _rememberedScale;
        private Vector3 _rememberedPosition;
        private Quaternion _rememberedRotation;
        private Vector2 _rememberedColliderOffset;

        private float _zPosDefault;
        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }

        public event Action<Card> OnClick;
        public event Func<Card, bool> CanPlay;
        public event Action<Card> OnBeginPlacing;
        public event Action<Card,Vector3> OnEndPlacing;

        public event Action<Card> OnMouseEnter;
        public event Action<Card> OnMouseExit;

        public abstract CardInfo Info { get; }
        protected abstract DragAnimations DragAnimations { get; }
        public int Cost { get; protected set; }
        public abstract bool ShouldRemainOnTable { get; }
        protected abstract CardInfoHolder InfoHolder { get; }

        protected PlayerId Owner;
        protected Player.Player Player;
        protected InputLockSystem InputLockSystem;
        protected BattleLog BattleLog;

        protected bool IsInputReadingLocked;
        protected bool IsPlaced;
        protected bool IsPlacing;
        protected bool IsDetailed;

        
        protected virtual void Awake()
        {
            CanPlay += (card) => true;
            _zPosDefault = transform.position.z; 
        }

        protected virtual void Start()
        {
            InfoHolder.SetMode(Player.CardViewMode);

            CardAudio.Init(this);
        }

        [Inject]
        private void Construct(InputLockSystem inputLockSystem, Player.Player player, BattleLog battleLog)
        {
            BattleLog = battleLog;
            InputLockSystem = inputLockSystem;
            InputLockSystem.AddInputReader(this);
            Owner = player.ID;
            Player = player;
        }

        protected virtual void OnDestroy()
        {
            InputLockSystem.RemoveInputReader(this);
        }

        public virtual void SetCost(int cost)
        {
            Cost = Mathf.Clamp(cost, 0, cost);
            InfoHolder.SetCost(Cost);
        }

        public virtual void Placed()
        {
            IsPlaced = true;
            SetToDefaultLayer();
            InfoHolder.SetMode(CardViewMode.Front);
            BattleLog.LogPlayCard(Info,Owner);            

            CardAudio.PlaceSound();
            
            transform.rotation = Quaternion.identity;
        }

        public virtual void NotPlaced()
        {
            if (Player.PlayCardInputSounds)
                CardAudio.ReturnSound();
        }

        public virtual void MouseDown(Vector3 mousePosition, PlayerId player)
        {            
            if (IsInputReadingLocked || player != Owner)
                return;


            if (!IsPlaced)
            {
                if (AskCanBePlayed())
                {
                    InvokeOnBeginPlacing();
                    transform.rotation = Quaternion.identity;
                    IsPlacing = true;
                    SetToTopLayer();
                }
                else
                {
                    if (Player.PlayCardInputSounds)
                        CardAudio.UnaccessSound();

                    return;
                }
            }

            if (Player.PlayCardInputSounds)
                CardAudio.ClickSound();
        }
        public virtual void MouseUp(Vector3 mousePosition, PlayerId player)
        {
            if (IsInputReadingLocked)
                return;

            OnClick?.Invoke(this);

            if (IsPlacing)
            {
                IsPlacing = false;

                Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePosition);
                worldMousePos.z = transform.position.z;

                InvokeOnEndPlacing(worldMousePos);
            }
        }
        public virtual void MouseDrag(Vector3 mousePosition, PlayerId player)
        {
            if (IsInputReadingLocked)
                return;

            if (IsPlacing)
            {
                PlacingCard(mousePosition);
            }

        }
        public virtual void MouseEnter(Vector3 mousePosition, PlayerId player)
        {
            if (player == Owner)
            {
                OnMouseEnter?.Invoke(this);
            }
        }
        public virtual void MouseExit(Vector3 mousePosition, PlayerId player)
        {
            if (player == Owner)
            {
                OnMouseExit?.Invoke(this);
            }
        }

        protected virtual void PlacingCard(Vector3 mousePosition)
        {
            Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePosition);
            worldMousePos.z = transform.position.z;

            if (DragAnimations != null)
            {
                DragAnimations.AnimateDrag(worldMousePos);
            }
            else
            {
                transform.position = worldMousePos;
            }
        }

        protected virtual void InvokeOnBeginPlacing()
        {
            OnBeginPlacing?.Invoke(this);
        }

        protected virtual void InvokeOnEndPlacing(Vector3 targetPos)
        {
            OnEndPlacing?.Invoke(this,targetPos);
        }

        protected virtual bool AskCanBePlayed()
        {
            return CanPlay(this);
        }

        public void SetEnabledInputReading(bool state)
        {
            IsInputReadingLocked = !state;            
        }

        protected void RememberTransform()
        {
            _rememberedPosition = transform.position;
            _rememberedRotation = transform.rotation;
            _rememberedScale = transform.localScale;

        }

        public void ShowDetailed(float targetY, Vector3 deltaScale)
        {
            if (!IsDetailed)
            {
                SetToTopLayer();
                
                Vector3 targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);
                Quaternion targetRotation = Quaternion.identity;

                InfoHolder.TransformView(targetPosition, targetRotation, deltaScale);
                
                IsDetailed = true;
            }
        }

        public void HideDetailed()
        {
            if (IsDetailed)
            {
                SetToDefaultLayer();
                
                InfoHolder.ReturnView();

                IsDetailed = false;
            }
        }

        public void SetToTopLayer()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, _zPosOnTop);

            InfoHolder.SetLayer(_topSortingLayer);
        }

        public void SetToDefaultLayer()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, _zPosDefault);

            InfoHolder.SetLayer(_defaulSortingtLayer);
        }
    }
}
