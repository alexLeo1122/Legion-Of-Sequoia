

namespace RPG.Character {
    interface ICharacterState 
        {
            //For setup State
            void Enter();
            //For update State
            void Update();
            //For clear "setup" State
            void Exit();
        }

}
