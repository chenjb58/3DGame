using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Mygame;

//总控制器
public class FirstController : MonoBehaviour, SceneController, UserAction {

	readonly Vector3 water_pos = new Vector3(0,0.5F,0);


	UserGUI userGUI;

	public CoastController fromCoast; //2个岸控制器
	public CoastController toCoast;
	public BoatController boat;//船控制器
	private MyCharacterController[] characters;//6个角色控制器

	void Awake() {
        //导演类
		Director director = Director.getInstance ();
		director.currentSceneController = this;
		userGUI = gameObject.AddComponent <UserGUI>() as UserGUI;
		characters = new MyCharacterController[6];
		loadResources ();
	}
    //实现SceneController接口，加载对象
    public void loadResources() {
		GameObject water = Instantiate (Resources.Load ("Perfabs/Water", typeof(GameObject)), water_pos, Quaternion.identity, null) as GameObject;
		water.name = "water";

		fromCoast = new CoastController ("from");
		toCoast = new CoastController ("to");
		boat = new BoatController ();
        //加载人物
		loadCharacter ();
	}

	private void loadCharacter() {
		for (int i = 0; i < 3; i++) {
			MyCharacterController cha = new MyCharacterController ("priest");
			cha.setName("priest" + i);
			cha.setPosition (fromCoast.getEmptyPosition ());
			cha.getOnCoast (fromCoast);
			fromCoast.getOnCoast (cha);

			characters [i] = cha;
		}

		for (int i = 0; i < 3; i++) {
			MyCharacterController cha = new MyCharacterController ("devil");
			cha.setName("devil" + i);
			cha.setPosition (fromCoast.getEmptyPosition ());
			cha.getOnCoast (fromCoast);
			fromCoast.getOnCoast (cha);

			characters [i+3] = cha;
		}
	}

    //实现Useraction接口，移动船
	public void moveBoat() {
		if (boat.isEmpty ())
			return;
		boat.Move ();
		userGUI.status = check_game_over ();
	}

    //实现Useraction接口，人物被点击动作
	public void characterIsClicked(MyCharacterController characterCtrl) {
        //人物对象在船上，送上岸
		if (characterCtrl.isOnBoat ()) {
			CoastController whichCoast;
			if (boat.get_to_or_from () == -1) { // to->-1; from->1
				whichCoast = toCoast;
			} else {
				whichCoast = fromCoast;
			}
            //将人物从船上抹去
			boat.GetOffBoat (characterCtrl.getName());
            //移动人物
			characterCtrl.moveToPosition (whichCoast.getEmptyPosition ());
            //人物登陆
			characterCtrl.getOnCoast (whichCoast);
            //陆地控制器接收
			whichCoast.getOnCoast (characterCtrl);

		} else {	//人物在岸上								// character on coast
			CoastController whichCoast = characterCtrl.getCoastController ();
            //船满载
			if (boat.getEmptyIndex () == -1) {		// boat is full
				return;
			}
            //点击船对面岸上的人无效
			if (whichCoast.get_to_or_from () != boat.get_to_or_from ())	// boat is not on the side of character
				return;
            //岸控制器释放
			whichCoast.getOffCoast(characterCtrl.getName());
            //人物移动
			characterCtrl.moveToPosition (boat.getEmptyPosition());
            //上船
			characterCtrl.getOnBoat (boat);
            //船控制器接收
			boat.GetOnBoat (characterCtrl);
		}
        //每次点击检查是否结束
		userGUI.status = check_game_over ();
	}

    //检查游戏是否结束，失败返回1，成功返回2，还能继续返回0
	int check_game_over() {	// 0->not finish, 1->lose, 2->win
		int from_priest = 0;
		int from_devil = 0;
		int to_priest = 0;
		int to_devil = 0;
        
		int[] fromCount = fromCoast.getCharacterNum ();
		from_priest += fromCount[0];
		from_devil += fromCount[1];

		int[] toCount = toCoast.getCharacterNum ();
		to_priest += toCount[0];
		to_devil += toCount[1];

		if (to_priest + to_devil == 6)		// win
			return 2;

		int[] boatCount = boat.getCharacterNum ();
		if (boat.get_to_or_from () == -1) {	// boat at toCoast
			to_priest += boatCount[0];
			to_devil += boatCount[1];
		} else {	// boat at fromCoast
			from_priest += boatCount[0];
			from_devil += boatCount[1];
		}
		if (from_priest < from_devil && from_priest > 0) {		// lose
			return 1;
		}
		if (to_priest < to_devil && to_priest > 0) {
			return 1;
		}
		return 0;			// not finish
	}
    //重置
	public void restart() {
		boat.reset ();
		fromCoast.reset ();
		toCoast.reset ();
		for (int i = 0; i < characters.Length; i++) {
			characters [i].reset ();
		}
	}
}
