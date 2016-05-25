<% @Page Language="VB" Explicit="True"%>
<%response.Expires=-1%>
<%response.Buffer=true%>
<%@ import Namespace="CardinalCommerce" %>
<!-- #Include File="CentinelConfig.aspx"-->
<%
'========================================================================================
'=  Cardinal Commerce (http://www.cardinalcommerce.com)
'=	demoTransfer.aspx
'=  Version 6.0
'=	03/01/2006
'=
'=	Form used to POST the PayPal Payment request to the Centinel Website for processing.
'=	Centinel will receive the request, construct the PayPal payment form and redirect
'=	the consumer to PayPal for payment.
'=
'========================================================================================
%>

<HTML>
<HEAD>
<SCRIPT Language="Javascript">
<!--
	function onLoadHandler(){
		document.frmLaunch.submit();
	}
//-->
</Script>
<Title>Processing Your Order</Title>
</HEAD>
<body onLoad="onLoadHandler();">
<br><br><br><br>
<center>
<FORM name="frmLaunch" method="Post" action="<%=Session("Centinel_ACSURL")%>">
<noscript>
	<br><br>
	<center>
	<font color="red">
	<h1>Processing Your PayPal Transaction</h1>
	<h2>JavaScript is currently disabled or is not supported by your browser.<br></h2>
	<h3>Please click Submit to continue	the processing of your transaction.</h3>
	</font>
	<input type="submit" value="Submit">
	</center>
</noscript>
<input type=hidden name="PaReq" value="<%=Session("Centinel_PAYLOAD")%>">
<input type=hidden name="TermUrl" value="<%=Cstr(Class1.DemoTermURL)%>">
<input type=hidden name="MD" value="<%=Session("Centinel_TransactionId")%>">
</FORM>
</center>
</BODY>
</HTML>
