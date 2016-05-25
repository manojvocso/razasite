<% @Page Language="VB" Explicit="True"%>
<%response.Expires=-1%>
<%response.Buffer=true%>
<%@ import Namespace="CardinalCommerce" %>
<%
'========================================================================================
'=  Cardinal Commerce (http://www.cardinalcommerce.com)
'=	demoOrderComplete.aspx
'=  Version 6.0
'=	03/01/06
'=
'=	This page represents the order completion page.  The page displays a message stating
'=	the status of the transaction and displays the order information.
'=
'========================================================================================
%>
<HTML>
<Head>
<Title>Demo - Order Summary</Title>
</Head>
<BODY>
<table width="100%" border="0" cellpadding="2" cellspacing="0">
  <tr valign="top"> 
    <td width="200" align="right"><img src="images\merchant_logo.gif"/></td>
	<td width="*"></td>
	<td align="left"></td>
  </tr>
  <tr height="7"> 
    <td colspan="3" valign="top" bgcolor="#e0e0e0" height="2">&nbsp;</td>
  </tr>
</table>
<center>
<h2 align="center">Order Summary</h2>
<% If Len(Session("Message")) > 0 then %>
<br/><br/>
	<font color="red"><h3 align="center"><%=Session("Message")%></h3></font>
<br/><br/>
<% 
   Session("Message") = ""
   End If %>

<table width="640" border="0" cellpadding="2" cellspacing="0">
	<tr><td bgcolor='#e0e0e0'><p align=center><b><font color='#000000' face='Arial,Verdana' size='2'>Order Details</font></b></td></tr>
	<tr><td>
	<table border='1' cellpadding='2' cellspacing='0' bordercolor='#e0e0e0'>
	<tr bgcolor='#FFFFFF'>
		<td><p align=center><b><font color='#000000' face='Arial,Verdana' size='2'> Description </font></b></td>
		<td><p align=center><b><font color='#000000' face='Arial,Verdana' size='2'> Quantity </font></b></td>
		<td><p align=center><b><font color='#000000' face='Arial,Verdana' size='2'> Unit Price </font></b></td>
		<td><p align=center><b><font color='#000000' face='Arial,Verdana' size='2'>Total</font></b></td>
	</tr>
	<tr bgcolor='#FFFFFF'>
		<td align="left" width="50%"><font face='verdana,arial' size=2><%=Session("Item_Description")%></td>
		<td width='10%' align=center><%=Session("Item_Quantity")%></td>
		<td align="right" width="10%"><font face='verdana,arial' size=2>$<%=Session("Formatted_Amount")%></font></td>
		<td align="right" width="10%"><font face='verdana,arial' size=2>$<%=Session("Formatted_Amount")%></font></td>
	</tr>
	<tr bgcolor='#FFFFFF'>
		<td align="right" colspan="3"><font face='verdana,arial' size=2>Order Total</td>
		<td align="right"><font face='verdana,arial' size=2>$<%=Session("Formatted_Amount")%></font></td>
	</tr>
	</table>
	</td></tr>
</table>

<br/>

<p align="center"><a href="demoStartPage.aspx"><b></b>Return to Start Page</b></a></p>
</center>

<%
'======================================================================================
' Remove All the Session Contents Once the Transaction Is Complete
'======================================================================================

Session.Contents.RemoveAll()

%>

</body>
</HTML>








