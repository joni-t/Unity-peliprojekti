#pragma strict

var Timer =0.0;



function Update () {

 
Timer+=Time.deltaTime;
GetComponent.<GUIText>().text=""+ Mathf.Round(Timer*100.00)/100.00;
}


 

