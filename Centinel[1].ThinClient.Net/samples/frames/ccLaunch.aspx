<% @Page Language="VB" Explicit="True"%>
<%response.Expires=-1%>
<%response.Buffer=true%>
<!-- #Include File="../CentinelConfig.aspx"-->
<%
'=====================================================================================
'=	Cardinal Commerce (http://www.cardinalcommerce.com)
'=	ccLaunch.aspx
'=	Version 5.0
'=	08/01/2005
'=
'=	Usage
'=		Form used to POST the transaction request to the External System.
'=		The External System will in turn display the authentication/payment information
'=		to the consumer within this location.
'=
'=		Note that the form field names below are case sensitive. For additional information
'=		please see the integration documentation.
'=
'=====================================================================================
%>
<HTML>
<HEAD>
<SCRIPT Language="Javascript">
<!--
	function onLoadHandler(){
		document.frmLaunchACS.submit();
	}
//-->
</Script>
</HEAD>
<body onLoad="onLoadHandler();">
<center>
<%
	'=====================================================================================
	' The Inline Authentication window must be a minimum of 390 pixel width by 
	' 400 pixel height.
	'=====================================================================================
%>
<FORM name="frmLaunchACS" method="Post" action="<%=Session("Centinel_ACSURL")%>">
<noscript>
	<br><br>
	<center>
	<font color="red">
	<h1>Processing your Transaction</h1>
	<h2>JavaScript is currently disabled or is not supported by your browser.<br></h2>
	<h3>Please click Submit to continue	the processing of your transaction.</h3>
	</font>
	<input type="submit" value="Submit">
	</center>
</noscript>
<input type=hidden name="PaReq" value="<%=Session("Centinel_PAYLOAD")%>">
<input type=hidden name="TermUrl" value="<%=Session("Centinel_TermURL")%>">
<input type=hidden name="MD" value="None">
</FORM>
</center>
</BODY>
</HTML>
