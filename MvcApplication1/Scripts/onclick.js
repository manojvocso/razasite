function getBlank(div){
document.getElementById(div).value='';
}
function getVal(div,str){
if(document.getElementById(div).value=="")
document.getElementById(div).value=str;
}