using Assets.Scripts.IAJ.Unity.DecisionMaking.GOB;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.GameManager
{
    //class that represents a world model that corresponds to the current state of the world,
    //all required properties and goals are stored inside the game manager
    public class CurrentStateWorldModel : FutureStateWorldModel
    {
        private Dictionary<string, Goal> Goals { get; set; } 

        public CurrentStateWorldModel(GameManager gameManager, List<Action> actions, List<Goal> goals) : base(gameManager, actions)
        {
            this.Goals = new Dictionary<string, Goal>();

            foreach (var goal in goals)
            {
                this.Goals.Add(goal.Name,goal);
            }

            this.Update(gameManager);
        }

        public void Update(GameManager gameManager)
        {
            this.InitializeProperties(gameManager);
        }

        public void Initialize()
        {
            this.ActionEnumerator.Reset();
        }

        protected void InitializeProperties(GameManager gameManager)
        {
            //Stats
            this.PropertiesArray[0] = gameManager.characterData.Mana;
            this.PropertiesArray[1] = gameManager.characterData.HP;
            this.PropertiesArray[2] = gameManager.characterData.MaxHP;
            this.PropertiesArray[3] = gameManager.characterData.XP;
            this.PropertiesArray[4] = gameManager.characterData.Time;
            this.PropertiesArray[5] = gameManager.characterData.Money;
            this.PropertiesArray[6] = gameManager.characterData.Level;
            this.PropertiesArray[7] = gameManager.characterData.CharacterGameObject.transform.position;
                                      
            //Potions                 
            this.PropertiesArray[10] = gameManager.potions.Any(x => x.name == Properties.HEALTHPOTION1);
            this.PropertiesArray[11] = gameManager.potions.Any(x => x.name == Properties.HEALTHPOTION2);
            this.PropertiesArray[12] = gameManager.potions.Any(x => x.name == Properties.MANAPOTION1);
            this.PropertiesArray[13] = gameManager.potions.Any(x => x.name == Properties.MANAPOTION2);
                                      
            //Chests                  
            this.PropertiesArray[15] = gameManager.chests.Any(x => x.name == Properties.CHEST1);
            this.PropertiesArray[16] = gameManager.chests.Any(x => x.name == Properties.CHEST2);
            this.PropertiesArray[17] = gameManager.chests.Any(x => x.name == Properties.CHEST3);
            this.PropertiesArray[18] = gameManager.chests.Any(x => x.name == Properties.CHEST4);
            this.PropertiesArray[19] = gameManager.chests.Any(x => x.name == Properties.CHEST5);
                                      
            //Enemies                 
            this.PropertiesArray[20] = gameManager.enemies.Any(x => x.name == Properties.SKELETON1);
            this.PropertiesArray[21] = gameManager.enemies.Any(x => x.name == Properties.SKELETON2);
            this.PropertiesArray[22] = gameManager.enemies.Any(x => x.name == Properties.ORC1);
            this.PropertiesArray[23] = gameManager.enemies.Any(x => x.name == Properties.ORC2);
            this.PropertiesArray[24] = gameManager.enemies.Any(x => x.name == Properties.DRAGON);
        }

        public override float GetGoalValue(string goalName)
        {
            return this.Goals[goalName].InsistenceValue;
        }

        public override void SetGoalValue(string goalName, float goalValue)
        {
            //this method does nothing, because you should not directly set a goal value of the CurrentStateWorldModel
        }

        public override void SetProperty(string propertyName, object value)
        {
            //this method does nothing, because you should not directly set a property of the CurrentStateWorldModel
        }

        public override int GetNextPlayer()
        {
            //in the current state, the next player is always player 0
            return 0;
        }
    }
}
