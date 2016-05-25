<% @Page Language="VB" Explicit="True"%>
<%response.Expires=-1%>
<%response.Buffer=true%>
<!-- #Include File="../CentinelConfig.aspx"-->
<%
'=====================================================================================
'=	CardinalCommerce (http://www.cardinalcommerce.com)
'=	ccFrame.aspx
'=  Version 5.0
'=	08/01/2005
'=
'=	Usage
'=		Page creates the framed layout for the payer authentication processing. 
'=
'=		Note that is is simply a sample layout. Your production implementation should take
'=		on your website's branding and layout. The only requirement is that there is adequate
'=		space (420 X 400) to display the authentication window to the consumer.
'=
'=====================================================================================
%>
<%
	'=====================================================================================
	' Check the transaction Id value to verify that this transaction has not already
	' been processed. This attempts to block the user from using the back button.
	'=====================================================================================

	If Session("Centinel_TransactionId") = "" then
	
			Session("Message") = "Order Already Processed, User Hit the Back Button"
			Response.redirect ("../ccLookup.asp")

	End If

%>
<html>
<HEAD>
<title>Processing Your Transaction</title>
<%
	'=====================================================================================
	' The Inline Authentication window (ecauth.asp) must be a minimum of 400 pixel 
	' width by 400 pixel height.
	'=====================================================================================
%>
<SCRIPT Language="Javascript">
<!--
	function displayJavascriptEnabledForm(){
		document.write('<FRAMESET ROWS="125,100%" frameborder="NO" border="0" framespacing="0">');
			document.write('<FRAME SRC="ccHeader.aspx" NAME=header SCROLLING=NO noresize>');
            document.write('<FRAME SRC="ccLaunch.aspx" NAME=WINDOW scrolling=auto noresize>');
		document.write('</FRAMESET>');
	}

	displayJavascriptEnabledForm()
//-->
</Script>
</HEAD>
<noscript>
	<FORM name="frmLaunchACS" method="Post" action="<%=Session("Centinel_ACSURL")%>">
		<br><br>
		<center>
		<font color="red">
		<h1>Processing Your Transaction</h1>
		<h2>JavaScript is currently disabled or is not supported by your browser.<br></h2>
		<h3>Please click Submit to continue	the processing of your transaction.</h3>
		</font>
		<input type="submit" value="Submit">
		</center>
		<input type=hidden name="PaReq" value="<%=Session("Centinel_PAYLOAD")%>">
		<input type=hidden name="TermUrl" value="<%=Session("Centinel_TermURL")%>">
		<input type=hidden name="MD" value="">
	</FORM>
</noscript>  
</HTML>
