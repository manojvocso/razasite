<% @Page Language="VB" Explicit="True" %>
<%response.Expires=-1%>
<%response.Buffer=true%>
<!-- #Include File="../CentinelConfig.aspx"-->
<%
'=====================================================================================
'= CardinalCommerce (http://www.cardinalcommerce.com)
'= ccHeader.aspx
'= Version 5.0 08/21/2005
'=
'= Usage
'=		Handles the display of Header information (logos, branding) to the
'=		consumer.
'=
'=====================================================================================

Dim headerText, imageSRC

If Session("Centinel_TransactionType") = "C" Then
	headerText = AuthenticationMessaging

	If Session("Centinel_PIType") = "VISA" Then
		imageSRC = VbVLogo
	Else If Session("Centinel_PIType") = "MASTERCARD"
		imageSRC = MCSCLogo
	Else If Session("Centinel_PIType") = "JCB"
		imageSRC = JCBLogo
	End If

Else If Session("Centinel_TransactionType") = "E" Then
	headerText = SecureEBillMessaging
	imageSRC = SEBLogo
Else If Session("Centinel_TransactionType") = "B" Then
	headerText = BMLMessaging
	imageSRC = BMLLogo
End If

%>
<HTML>
<HEAD></HEAD>
<BODY>
<table width="100%" border="0" cellpadding="1" cellspacing="0" >

  <tr valign="top"> 
	<td width="20%" align="right"><img src="<%=MerchantLogo%>"/></td>
	<td width="*"></td>
	<td width="20%"></td>
  </tr>
  <tr><td colspan="3"><hr/></td></tr>
  <tr valign="top"> 
	<td width="20%" align="right"></td>
	<td width="60%" align="center">
	<font size="2" color="#000000" face="Arial, Helvetica, sans-serif">
	<%=headerText%>	
	</font>
	</td>
	<td width="20%"><img src="<%=imageSRC%>"/></td>
  </tr>
</table>
</BODY>
</HTML>