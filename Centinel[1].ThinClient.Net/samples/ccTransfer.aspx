<% @Page Language="VB" Explicit="True" %>
<%response.Expires=-1%>
<%response.Buffer=true%>
<%
'=====================================================================================
'= CardinalCommerce (http://www.cardinalcommerce.com)
'= ccTransfer.aspx
'= Version 6.0 03/21/2006
'=
'= Usage
'=		Form used to POST the PayPal Payment request to the ACSURL for processing.
'=
'=		Note that the form field names below are case sensitive. For additional information
'=		please see the merchant integration documentation.
'=
'=====================================================================================
%>
<!-- #Include File="CentinelConfig.aspx"-->
<%@ import Namespace="CardinalCommerce" %>

<HTML>
<HEAD>
<TITLE>Processing Your Transaction</TITLE>
<SCRIPT Language="Javascript">
<!--
	function onLoadHandler(){
		document.frmLaunch.submit();
	}
//-->
</Script>
</HEAD>
<body onLoad="onLoadHandler();">
<br><br><br><br>
<center>
<FORM name="frmLaunch" method="Post" action="<%=Session("Centinel_ACSURL")%>">
<noscript>
	<br><br>
	<center>
	<font color="red">
	<h1>Processing Your Transaction</h1>
	<h2>JavaScript is currently disabled or is not supported by your browser.<br></h2>
	<h3>Please click Submit to continue the processing of your transaction.</h3>
	</font>
	<input type="submit" value="Submit">
	</center>
</noscript>
<input type=hidden name="PaReq" value="<%=Session("Centinel_PAYLOAD")%>">
<input type=hidden name="TermUrl" value="<%=Cstr(TermURL)%>">
<input type=hidden name="MD" value="<%=Session("Centinel_TransactionId")%>">
</FORM>
</center>
</BODY>
</HTML>
