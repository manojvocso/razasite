function CreateUpdateCookie(c_name,c_value)
{
    alert(c_name)
    alert(c_value)
    var pthDomain = ";path=/;domain=.verizon.com";
    var expdays = 30;	
	var expdate=new Date();
	expdate.setDate(expdate.getDate()+expdays);
	/*alert(expdate.toGMTString())
	alert("UTC: " + expdate.toUTCString())*/
	document.cookie=c_name+ "=" +c_value+ pthDomain + ";expires="+expdate.toGMTString(); 
}

function changeLocation()
    {
	var obj = document.getElementById ("RVOchngLocIFrame")
//	obj.src="http://www25.verizon.com/content/learnshop/testpage.aspx"
	obj.src="https://" + document.domain + "/FORYOURHOME/GOFLOW/newconnect/changelocations.aspx?change=Y"

    }

/*******To Include Trim() functionality********/
if(typeof String.prototype.trim !== 'function') 
{ 
    String.prototype.trim = function() 
    { 
        return this.replace(/^\s+|\s+$/, '');  
    } 
}
/**********************************************/

function getRVCookie(cookieName)
{
    if (document.cookie.length>0)
    {
        cStart=document.cookie.indexOf(cookieName + "=");
        if (cStart!=-1)
        { 
            cStart=cStart + cookieName.length+1; 
            cEnd=document.cookie.indexOf(";",cStart);
            if (cEnd==-1) cEnd=document.cookie.length;
                return unescape(document.cookie.substring(cStart,cEnd));
        } 
    }
    return "";
}


function setRVCookie(c_name,value)
{
   var pthDomain = ";path=/;domain=.verizon.com";
   //var pthDomain = ""; 

//document.cookie="VzBanner=PRODUCTIN="+VzBphone+VzBfiosdigital+VzBfiosinternet+VzBfiostv+VzBwireless+VzBhsi+VzBdtv+"&"+offerid+pthDomain+";expires="+exdate.toGMTString();

//document.cookie="VzBanner="dsfsdf&"+pthDomain+";expires="+exdate.toGMTString();
    
	var expdays = 30;	
	var expdate=new Date();
	expdate.setDate(expdate.getDate()+expdays);
	/*alert(expdate.toGMTString())
	alert("UTC: " + expdate.toUTCString())*/
	
	//document.cookie=c_name+ "=" +value + ";expires="+expdate.toGMTString(); 
	document.cookie=c_name+ "=" +value+ pthDomain + ";expires="+expdate.toGMTString(); 
	var cookieVal = getRVCookie('RecentlyVisitedOffers');
	var TotURLs = cookieVal.split('^')
	
	if(TotURLs.length>10)
	{
	    var arrTemp = new Array();
	    var str = "";
	    for(i=0;i<TotURLs.length;i++)
	    {
	        if(TotURLs[i].trim().length>0)
	            arrTemp[i] = TotURLs[i];
	        if(i==9)
	            break
	    }
	    for(i=0;i<arrTemp.length;i++)
	    {
	        if(str=="")
	        {
	            str = arrTemp[0] + "^";
	        }
	        else
	        {
	            str = str + arrTemp[i] + "^";
	        }
	    }
	    document.cookie="RecentlyVisitedOffers=" + str;
	}
}

function CaptureId(obj)
{ 
    var title_price = "";
    title_price = obj.split(',') //title_price[1]--> Title, title_price[2]--> Price
    var cookieContent = "";
    var newCookieContent = "";
	var pthDomain = ";path=/;domain=.verizon.com";
	var expdays = 30;	
	var expdate=new Date();
	expdate.setDate(expdate.getDate()+expdays);

    obj = obj.replace(/<!--LN-->/g,"")
    obj = obj.replace(/<!--TS-->/g,"")
    obj = obj.replace(/<!--SP-->/g,"")
    if(obj.toLowerCase().indexOf('not')!=-1 && obj.toLowerCase().indexOf('available')!=-1 )   
        return
    var cookieVal = getRVCookie('RecentlyVisitedOffers');
    
    if(cookieVal.trim().length>0)
    {
        cookieContent = cookieVal.split('^')
        for(var i=0;i<=cookieContent.length-1;i++)
        {
            if(cookieContent[i].length>0)
            {

                
                if(cookieContent[i].toLowerCase().indexOf(title_price[1].toLowerCase()+","+title_price[2].toLowerCase())!=-1 || cookieContent[i].toLowerCase().indexOf(title_price[1].toLowerCase()+",$"+title_price[2].toLowerCase())!=-1)
                {
                    
                }
                else
                {
			
                    newCookieContent = newCookieContent + cookieContent[i] + "^";
                }
            }
        }
        cookieVal = newCookieContent;
    }
    
    
    
    
    
    
    
    
    
    //var urlInfo = obj + '*' + document.location;

    var urlInfo = obj + '*' + (document.location + "").substring((document.location + "").indexOf("/",13));

    
    //For tacking HBX we set this below Cookie, Start
    var strURL = document.location + ""; 
    var strGHP = strURL.split('/')
    if(strGHP.length==3 || strGHP.length==4)
    {
        document.cookie="CustTrackPage=" + "GHP" + pthDomain + ";expires="+expdate.toGMTString();
    }
    else if(strURL.toLowerCase().indexOf('aboutfios')!=-1)
    {
        document.cookie="CustTrackPage=" + "AboutFiOS" + pthDomain + ";expires="+expdate.toGMTString();
    }
    else if(strURL.toLowerCase().indexOf('bundles')!=-1)
    {
        document.cookie="CustTrackPage=" + "Bundles" + pthDomain + ";expires="+expdate.toGMTString();
    }
    else if(strURL.toLowerCase().indexOf('fiostv')!=-1)
    {
        document.cookie="CustTrackPage=" + "FiOSTV" + pthDomain + ";expires="+expdate.toGMTString();
    }
    else if(strURL.toLowerCase().indexOf('fiosinternet')!=-1)
    {
        document.cookie="CustTrackPage=" + "FiOSInternet" + pthDomain + ";expires="+expdate.toGMTString();
    }
    else if(strURL.toLowerCase().indexOf('highspeedinternet')!=-1)
    {
        document.cookie="CustTrackPage=" + "HSI" + pthDomain + ";expires="+expdate.toGMTString();
    }
    else if(strURL.toLowerCase().indexOf('phone')!=-1)
    {
        document.cookie="CustTrackPage=" + "Phone" + pthDomain + ";expires="+expdate.toGMTString();
    }
    else if(strURL.toLowerCase().indexOf('directv')!=-1)
    {
        document.cookie="CustTrackPage=" + "DirecTV" + pthDomain + ";expires="+expdate.toGMTString();
    }
    //end
    /*alert(document.cookie)
    return*/
    
    if(cookieVal.indexOf(urlInfo) == -1)
        setRVCookie('RecentlyVisitedOffers',urlInfo+'^' +cookieVal);
		
}
//CaptureId('testing purpose')