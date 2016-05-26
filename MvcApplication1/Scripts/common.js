var req ;

var t1;

var cufOff = false;

var xpDecisions="";

var cid1="";

var cid2="";

var cid3="";

var cid4="";

var objAjax=null;

var clickreq;

var TryCountAjax = 0;

var TryCountD = 0;



var iCo = 0;



function trim(str) { return str.replace(/^\s+|\s+$/g, ""); };



function hbxPers()

{





  var hbxRet    = getCookie("hbxRet");         //alert(hbxRet );

  var hbxBusUnit= getCookie("BusinessUnit");   //alert(hbxBusUnit);

  var hbxLQ     = getSubCookie("VzLQPro","LQ");//alert(hbxLQ );





  //retun visit

  var cv = _hbEvent("cv");

  if(hbxBusUnit != null && (hbxRet == null || hbxRet == ""))

  {

   if(hbxLQ==null || hbxLQ=="")

    {

      cv.c17 = "PreQualNever";

    }

    else

    {

     cv.c17 = "PostQualReturn";

    }

  }

  else  // initial visit

  {

   if(hbxLQ==null || hbxLQ=="")

    {

      cv.c16 = "PreQual";

    }

    else

    {

     cv.c16 = "PostQual";

    }

  }





}



function getElementByClassAndId(tName,className,id,obj)

{

	var lists = obj.getElementsByTagName(tName);

	for(var i=0;i<lists.length;i++)

	{

		if(className != '')

		{

			if((lists[i].className == className) && (lists[i].id == id))

				return lists[i];

		}

		else

		{

			if(lists[i].id == id)

				return lists[i];

		}

	}

	return null;

}





function CheckLoadedAjax()

{

	if(document.body)	

	{

		try

		{

			pattern_layers = new layers({launchers: $ES('.launcher')});				

		}

		catch(ec)

		{

			Excp=ec;

			if(TryCount<4)

			{

				TryCount=TryCount+1;

				setTimeout(CheckLoaded,2000);					

			}

		}



	}

	else

	{			

		setTimeout(CheckLoaded,2000)

	}

}



	

function sPromo()

{

    var showpromocontent;

    

    try

    {

        

        if(getFiosCookie('showpromo')!=null)

        {

            showpromocontent = getCookie('showpromo');

        }

        //showpromocontent = 'Y';

        obj1 = document.getElementById('marquee_container');

        if(showpromocontent=='Y')

        {

            _hbLink('heropromo_bnr1_pos4_onload_a');

            if(obj1 != null)

            {

                getElementByClassAndId('div','threefour_page_offer','content4',obj1).style.display = 'block';

                getElementByClassAndId('span','','button4',obj1).style.display = 'block';

                getElementByClassAndId('span','','button5',obj1).style.display = 'none';

                getElementByClassAndId('div','threefour_page_offer','content5',obj1).style.display = 'none';

            }

        } else {

            _hbLink('heropromo_bnr1_pos4_onload_b');

            if(obj1 != null)

            {

                getElementByClassAndId('div','threefour_page_offer','content5',obj1).style.display = 'block';

                getElementByClassAndId('span','','button5',obj1).style.display = 'block';

                getElementByClassAndId('div','threefour_page_offer','content4',obj1).style.display = 'none';

                getElementByClassAndId('span','','button4',obj1).style.display = 'none';

            }

        }

        //var marqueecontainer = document.getElementById('marquee_container');

    }

    catch(e)

    {

            if(TryCountAjax<4)

			{

				TryCountAjax=TryCountAjax+1;

				setTimeout(sPromo,2000);					

			}

    }

}





//Ajax Call xmlhttprequest -Logging start 

//var chkXplus1log = "<%=chkXplus1Log%>";

var chkXplus1log = 'true';



function CallAjax(placements)

{

	if(chkXplus1log == 'true')

	{

	    Createobj(); 

            xlogParam = "xLogParam="+escape(placements) + "**" + escape(xp1src) ;

            

      

	    objAjax.open("post","/content/verizonglobalhome/xplus1log.aspx",  false);

	    objAjax.setRequestHeader('Content-Type','application/x-www-form-urlencoded');

	    objAjax.send(placements);

		

	    return objAjax.responseText;

	}

}

function Createobj()

{

    try

    {

        objAjax =new ActiveXObject("Msxml2.XMLHTTP");

    }

    catch(e)

    {

        try

        {

            objAjax =new ActiveXObject("Microsoft.XMLHTTP");

        }

        catch(e1)

        {

            try

            {

                objAjax =new XMLHttpRequest();

            }

            catch(e2)

            {

                objAjax =null;

            }

        }

    }

}

//Ajax Call xmlhttprequest -Logging End

/*function sPromo()

{

    var showpromocontent;

	

	    if(getFiosCookie('showpromo')!=null)

        {

            showpromocontent = getCookie('showpromo');

            //alert('showpromocontent' + showpromocontent);

        }

	

	     

        if(showpromocontent=='Y')

        {

            

            _hbLink('heropromo_bnr1_pos4_onload_a');

        	document.getElementById('content4').style.display = 'block';

            document.getElementById('button4').style.display = 'block';

            document.getElementById('button5').style.display = 'none';

            document.getElementById('content5').style.display = 'none';

            //document.getElementById('content5').innerHTML = '';

        } else {

            _hbLink('heropromo_bnr1_pos4_onload_b');

        	document.getElementById('content5').style.display = 'block';

        	document.getElementById('button5').style.display = 'block';

		    document.getElementById('content4').style.display = 'none';

            document.getElementById('button4').style.display = 'none';

            

        }

}*/



function updateUID(name)

{



    adr1 = name;

    if(document.getElementById('yourLocation'))

        document.getElementById('yourLocation').innerHTML = adr1 + ", " + st + " " + zc;

}



function cCookie(t)

{

    if(t!= null && t.length > 0)

        return t;

    else 

        return "";

}



function f()

{

var NPA;

var NXX;

var LAST;



var zc = cCookie(getSubCookie('vzapps','Zip Code / Postal Code :CODE'));

var st = cCookie(getSubCookie('vzapps','STATE'));

if(st == 'V1' || st == 'V2')

    st = 'VA';

if(st == 'P1' || st == 'P2')

    st = 'PA';

var adr1 = getSubCookie('vzapps','adr1');

if(adr1 != null && adr1.length > 0)

{

    url = "/content/verizonglobalhome/getuid.aspx?uid="+adr1;

    document.write("<scr"+"ipt type=\"text/javascript\" src="+ url +"><\/scr"+"ipt>");



}

else

{

    if(cCookie(getSubCookie('vzapps','NPA')).length > 0)

        adr1 = cCookie(getSubCookie('vzapps','NPA')) + "-" + cCookie(getSubCookie('vzapps','NXX'))+ "-" + cCookie(getSubCookie('vzapps','LAST')) + " ,";



   

}

if(adr1 == null) adr1 = '';

if(document.getElementById('yourLocation'))

        document.getElementById('yourLocation').innerHTML =  adr1 + st + " " + zc;



}



/*

GHP.addLoadEvent(function() {



   var tCookie = getSubCookie('vzapps','STATE');

   if(tCookie == null || tCookie.length == 0)

    sPromo();

    



	var m = new GHP.Marquee(document.getElementById('marquee_container'));

	var tick = document.getElementById('tickbox');

	GHP.accordionInit();

	tick.onmouseover = function() {

		stopCount();	

	};

	tick.onmouseout = function() {

		timedCount();

	}

	startScroller();

});



*/



//srs

var TryCount1 = 0;

var CheckContainerLoaded=false;

function addLoadEvent_GHP()

{

   try

   {

       var tCookie = getSubCookie('vzapps','STATE');

       if(tCookie == null || tCookie.length == 0)

        sPromo();

        



	    var m = new GHP.Marquee(document.getElementById('marquee_container'));

	    var tick = document.getElementById('tickbox');

	    GHP.accordionInit();

	

	    tick.onmouseover = function() {

		    stopCount();	

	    };

	    tick.onmouseout = function() {

		    timedCount();

	    }

	    startScroller();

	    CheckContainerLoaded=true;

	}

	catch(e)

	{

       		 if(TryCount1 < 10)

		        {

		            TryCount1=TryCount1+1;

		            setTimeout(addLoadEvent_GHP,2000);					

		        }

	}

}

addLoadEvent_GHP();



//srs





function addLoadEvent_test()

{ 

/*

layers.implement(new Options, new Events);



if ($ES('.launcher'))

	{

		pattern_layers = new layers({

			launchers: $ES('.launcher')

		});

	}

*/



//document.getElementById('allJS').src = "/content/verizonglobalhome/includes/javascript/all.js";

    //sPromo();

    //var percent_marquee=document.getElementById("content4").getAttribute("name");

//randomcontentdisplay.init()



    var m = new GHP.Marquee(document.getElementById('marquee_container'));

	var tick = document.getElementById('tickbox');

	GHP.accordionInit();

	//for(var i =0;i<4;i++)

    //m.go(i);

    //m.go(0);

	tick.onmouseover = function() {

		stopCount();	

	};

	tick.onmouseout = function() {

		timedCount();

	}

	

}

uDef = false;



function loadDHTML()

{

    if(!cufOff)

    {

    document.getElementById('marquee_container').innerHTML = document.getElementById('marquee_table_content').innerHTML;

    document.getElementById('marquee_table_content').innerHTML = "";

    setTimeout(startScroller(),100);

f();

   }

}

//setTimeout(loadDHTML,5000);

var cVals;

function loadHTML()

{

    try{

    if(req.readyState == 4)

    {

        t1 = req.responseText;

        

       // CallAjax("status=0&xp1Req=&xp1Res="+   +"&error=&pname=GHP&tname="); //escape(req.responseText)

        

        if(t1.indexOf('Server Error')!=-1)

            loadDHTML();

        else

        {

        var aT = t1.split('$$$$$');

       

        document.getElementById('marquee_container').innerHTML = aT[0];

        

        document.getElementById('rVisited').innerHTML = aT[1];

	if(document.getElementById('yourlocation'))

        document.getElementById('yourlocation').innerHTML = aT[2];

        cVals = aT[2];

/*

if(trim(aT[2]).length > 0)

	{

		document.getElementById('yourlocation').style.paddingTop = '5px';

		document.getElementById('chgloclnk').style.display = '';

		if(document.getElementById('personal') != null)document.getElementById('personal').style.display='';

	}

	else

	{



		if(document.getElementById('personal') != null)document.getElementById('personal').style.display='none';

		if(document.getElementById('tn_mv').getElementsByTagName('span').length > 0)

		{

			var objS = document.getElementById('tn_mv').getElementsByTagName('span')[0];

			if(objS.style.color.indexOf('119') != -1)

				objS.style.display = 'none';

				

		}

	//	document.getElementById('chgloclnk').style.display = 'none';

	//	document.getElementById('yourlocation').style.display = 'none';



	} */

var showpromocontent;



	

	    if(getFiosCookie('showpromo')!=null)

        {

            showpromocontent = getCookie('showpromo');

            //alert('showpromocontent' + showpromocontent);

        }



 if(showpromocontent=='Y')

        {

            

            _hbLink('heropromo_bnr1_pos4_onload_a');

        	//document.getElementById('content4').style.display = 'block';

           // document.getElementById('button4').style.display = 'block';

           // document.getElementById('button5').style.display = 'none';

          //  document.getElementById('content5').style.display = 'none';

        } else {

            _hbLink('heropromo_bnr1_pos4_onload_b');

        	//document.getElementById('content5').style.display = 'block';

        //	document.getElementById('button5').style.display = 'block';

	//	    document.getElementById('content4').style.display = 'none';

           // document.getElementById('button4').style.display = 'none';

            

        }

        

        //document.getElementById('allJS').src = "/content/verizonglobalhome/includes/javascript/all.js";

        //setTimeout(startScroller(),100);

        }

    }}

    catch(e)

    {

        cufOff = true;

        loadDHTML();

    }

}



function getPromo(url) {

 

	req = false;



    if(window.XMLHttpRequest && !(window.ActiveXObject)) {

    	try {

			req = new XMLHttpRequest();

        } catch(e) {

			req = false;

        }



    } else if(window.ActiveXObject) {

       	try {

        	req = new ActiveXObject("Msxml2.XMLHTTP");

      	} catch(e) {

        	try {

          		req = new ActiveXObject("Microsoft.XMLHTTP");

        	} catch(e) {

          		req = false;

          		cufOff = true;

          		loadDHTML();

        	}

		}

    }

	if(req) {		

		req.onreadystatechange = loadHTML;

		var d=new Date();

		var t=d.getTime();

var st = cCookie(getSubCookie('vzapps','STATE'));

if(st.length > 0) st="&state="+st;



		//////////

//for testing purpose only

var strjss=

"{"+

     "\"xp1_placement1\": { "+

          "\"creativeID\":GHP_OT_FTP1_FLEVE\","+

          "\"clickURL\": \"https://stg.xp1.ru4.com/click?_o=15772&_n=62795&_c=1150261&_x=1150264&_b=1150331&_d=0&_g=1150399&_a=1150398&_p=1150265&_s=0&_pm=0&_pn=0&_pl=0&redirect=\""+

     "}," +

    "	\"xp1_placement2\": {"+

         " \"creativeID\": \"GHP_FT_WID_SPO\","+

         " \"clickURL\": \"https://stg.xp1.ru4.com/click?_o=15772&_n=62795&_c=1150261&_x=1150264&_b=1150342&_d=0&_g=1150399&_a=1150398&_p=1150266&_s=0&_pm=0&_pn=0&_pl=0&redirect=\""+

    " },"+

    " \"xp1_placement3\": {"+

        "  \"creativeID\": \"GHP_PT_FV\","+

          "\"clickURL\": \"https://stg.xp1.ru4.com/click?_o=15772&_n=62795&_c=1150261&_x=1150264&_b=1150345&_d=0&_g=1150399&_a=1150398&_p=1150267&_s=0&_pm=0&_pn=0&_pl=0&redirect=\""+

    " },"+

     "\"xp1_placement4\": {"+

         " \"creativeID\": \"GHP_OT_FTP1_NYFB\","+

          "\"clickURL\": \"https://stg.xp1.ru4.com/click?_o=15772&_n=62795&_c=1150261&_x=1150264&_b=1150348&_d=0&_g=1150399&_a=1150398&_p=1150268&_s=0&_pm=0&_pn=0&_pl=0&redirect=\""+

     "}"+

"}";



//xpDecisions=strjss;

//alert(xpDecisions);

///////////



  if(strVzP2=="N" || strVzP2=="")

  { 

  

      xpDecisions=null;     

  }

  

  



		if(xpDecisions == null)

		{

			req.open("GET", url+'?a='+escape(t)+st, true);

			req.send("");

			

		}

		else

		{  

			req.open("POST", url, true);//+'?a='+escape(t)+st

			req.setRequestHeader('Content-Type','application/x-www-form-urlencoded');	

			xResult1 = "xResult="+escape(xpDecisions+ "^" + strRet);           

			

			req.send(xResult1);

		}

	}

}







function getArgs() 

{

var args = new Object();

var query = location.search.substring(1);

var pairs = query.split("&");

for(var i = 0; i < pairs.length; i++) {

var pos = pairs[i].indexOf('=');

if (pos == -1) continue;

var argname = pairs[i].substring(0,pos);

var value = pairs[i].substring(pos+1);

args[argname] = unescape(value);

}

return args;

}





function GetCreativesFromQS()

{



 var arg=getArgs() ;

 

 cid1 = arg.id1;

 cid2= arg.id2;

 cid3 = arg.id3;

 cid4=arg.id4;

}





var xpDecisions = null;

function getXPlus1Promo(placements)

{ 



GetCreativesFromQS();

var qsDecision="";



if(cid1 != null && cid1 !="")

{

 qsDecision=

"{"+

     "\"xp1_placement1\": { "+

          "\"creativeID\": \""  + cid1+ "\","+

          "\"clickURL\": \"https://stg.xp1.ru4.com/click?_o=15772&_n=62795&_c=1150261&_x=1150264&_b=1150331&_d=0&_g=1150399&_a=1150398&_p=1150265&_s=0&_pm=0&_pn=0&_pl=0&redirect=\""+

     "}," +

    "	\"xp1_placement2\": {"+

         " \"creativeID\": \""+ cid2+"\","+

         " \"clickURL\": \"https://stg.xp1.ru4.com/click?_o=15772&_n=62795&_c=1150261&_x=1150264&_b=1150342&_d=0&_g=1150399&_a=1150398&_p=1150266&_s=0&_pm=0&_pn=0&_pl=0&redirect=\""+

    " },"+

    " \"xp1_placement3\": {"+

        "  \"creativeID\": \"" +cid3+"\","+

          "\"clickURL\": \"https://stg.xp1.ru4.com/click?_o=15772&_n=62795&_c=1150261&_x=1150264&_b=1150345&_d=0&_g=1150399&_a=1150398&_p=1150267&_s=0&_pm=0&_pn=0&_pl=0&redirect=\""+

    " },"+

     "\"xp1_placement4\": {"+

         " \"creativeID\": \""+cid4+"\","+

          "\"clickURL\": \"https://stg.xp1.ru4.com/click?_o=15772&_n=62795&_c=1150261&_x=1150264&_b=1150348&_d=0&_g=1150399&_a=1150398&_p=1150268&_s=0&_pm=0&_pn=0&_pl=0&redirect=\""+

     "}"+

"}";



  xpDecisions =qsDecision;

}

else

 {

  xpDecisions = placements;

 }







 getPromo('/content/verizonglobalhome/gPromo.aspx');

 



}



//getPromo('/content/verizonglobalhome/gPromo.aspx');







//click URL from x+1

// redirect_url from db

function xp1ClickRedir(click_url) 

{    



    clickreq = false;

    try

    {



 if(window.XMLHttpRequest && !(window.ActiveXObject)) {

    	try {

			clickreq = new XMLHttpRequest();

        } catch(e) {

			clickreq = false;

        }



    } else if(window.ActiveXObject) {

       	try {

        	clickreq = new ActiveXObject("Msxml2.XMLHTTP");

      	} catch(e) {

        	try {

          		clickreq = new ActiveXObject("Microsoft.XMLHTTP");

        	} catch(e) {

          		clickreq = false;

          		

        	}

		}

    }

	if(clickreq) 

		{

                 var xpix = "http://s.xp1.ru4.com/images/pixel.gif";

		        /*clickreq.onreadystatechange = function xp1Redirect() {

			     document.location = redirect_url;

 				   };*/

                 var clickurl =click_url+xpix;

		        clickreq.open("GET", clickurl , false);

		        clickreq.send(null);	

                }



//document.location = redirect_url;





}





catch(e)

{

  // alert("caught exception");

 //  document.location = redirect_url;

}



}





function clearcookies()

{

    var thecookie = new Array();

    thecookie[0]='VzGlobal_s';

    thecookie[1]='VzGlobal_10';

    thecookie[2]='vzapps';

    thecookie[3]='VzLQPro';

    thecookie[4]='VzGlobal_p';

    thecookie[5]='VzCart'



    var new_date = new Date();

    new_date = new_date.toGMTString();

    //var thecookie = document.cookie.split(";");

    for(var i=0;i<thecookie.length;i++)

    {

	document.cookie= thecookie[i] + ";path=/;domain=.verizon.com;expires="+ new_date;

    }

}







///////////////////////////////////

function fnAjaxCallBTagScript(url)

{





if(url=='Sports')	    url = 'http://s.xp1.ru4.com/meta?_o=15772&_t=behavioral&ssv_001=sports';

if(url=='Movies/TV')	url = 'http://s.xp1.ru4.com/meta?_o=15772&_t=behavioral&ssv_001=movies-tv';

if(url=='Family')	    url = 'http://s.xp1.ru4.com/meta?_o=15772&_t=behavioral&ssv_001=family';

if(url=='TV')	        url = 'http://s.xp1.ru4.com/meta?_o=15772&_t=behavioral&ssv_002=tv';

if(url=='Internet')	    url = 'http://s.xp1.ru4.com/meta?_o=15772&_t=behavioral&ssv_002=internet';

if(url=='Phone')	    url = 'http://s.xp1.ru4.com/meta?_o=15772&_t=behavioral&ssv_002=phone';

if(url=='Bundles')	    url = 'http://s.xp1.ru4.com/meta?_o=15772&_t=behavioral&ssv_002=bundles';



	bTagreq = false;



    if(window.XMLHttpRequest && !(window.ActiveXObject)) 

    {

        try 

        {

		    bTagreq = new XMLHttpRequest();

        } 

        catch(e) 

        { 

			bTagreq = false;

        }

    } 

    else if(window.ActiveXObject) 

    {

       	try 

       	{

        	bTagreq = new ActiveXObject("Msxml2.XMLHTTP");

      	} 

      	catch(e) 

      	{ 

        	try 

        	{

          		bTagreq = new ActiveXObject("Microsoft.XMLHTTP");

        	} 

        	catch(e) 

        	{

          		bTagreq = false;

        	}

		}

    }

    

	if(bTagreq) 

	{		

		return;

		var d=new Date();

		var t=d.getTime();

		bTagreq.open("GET", url , false);

		bTagreq.send(null);

		

	}

}

/////////////////////////////////////////



//hbxPers();