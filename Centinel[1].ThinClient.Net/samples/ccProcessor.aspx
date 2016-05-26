<% @Page Language="VB" Explicit="True"%>
<%response.Expires=-1%>
<%response.Buffer=true%>
<!-- #Include File="CentinelConfig.aspx"-->
<!-- #Include File="CentinelUtility.aspx"-->

<%@ import Namespace="CardinalCommerce" %>

<%'======================================================================================
'=  Cardinal Commerce (http://www.cardinalcommerce.com)
'=  ccProcessor.aspx
'=  Version 6.0 03/01/2006
'=	Usage
'=		A sample or template integration of the Centinel Thin Client. The samples follows a basic inline 
'=		authentication approach and provides the sample that utilizes the Thin Client for communication
'=		with the MAPS servers. 
'=
'=	Note 
'=		This is ONLY A SAMPLE INTEGRATION. It has been tested under
'=		Internet Explorer(6.0), Mozilla (1.3, 1.7), Netscape (4.08,7.0), a production integration
'=		may require additional browser support and should be tested with respect to those guidelines.
'=
'=	Transaction API
'=		Please reference the current transaction API documentation for a complete list of
'=		all required and optional message elements.
'=
'=	Return Parameters Checked and Used for Processing
'=
'=		Enrolled	- (Y/N/U) Indicates if the user is enrolled in the payer authentication program
'=		ACSUrl		- The fully qualified Url of the location of the ACS Server (Card Issuer Server)
'=		Payload		- The encrypted Message payload, to be interperted by the Card Issuer's Server
'=		ErrorNo		- If an error was encountered while processing the Enrollment Request message
'=				  the error number will be available. Error Codes are distributed with the 
'=				  Thin Client Documentation.
'=		ErrorDesc	- Indicates the ErrorDesc, if ErrorNo is not '0'
'======================================================================================%>
<%

	Dim strCardEnrolled, strErrorNo, strErrorDesc, strTransactionId, strECI, strTransactionType
	
	Dim centinelRequest As New CentinelRequest()
	Dim centinelResponse As New CentinelResponse()
	
	strTransactionType = request("txn_type")
	Session("Centinel_TermURL") = TermURL
	

	'======================================================================================
	' Use the thin client Add method to construct the cmpi_lookup message. 
	' Additional API documentation is available within the Thin Client Documentation.
	'
	' Note that the MessageVersion, ProcessorId, MerchantId values are defined within the
	' CentinelConfig.aspx include file.
	'======================================================================================

	centinelRequest.Add ("Version", Cstr(MessageVersion))
	centinelRequest.Add ("MsgType", "cmpi_lookup")
	centinelRequest.Add ("ProcessorId", Cstr(ProcessorId))
	centinelRequest.Add ("MerchantId", Cstr(MerchantId))
	centinelRequest.Add ("TransactionPwd", Cstr(TransactionPwd))
	centinelRequest.Add ("TransactionType", Cstr(strTransactionType))
	centinelRequest.Add ("Amount", Cstr(request("amount")))
	centinelRequest.Add ("CurrencyCode", Cstr(request("currency_code")))
	centinelRequest.Add ("CardNumber", Cstr(request("card_number")))
	centinelRequest.Add ("CardExpMonth", Cstr(request("expr_mm")))
	centinelRequest.Add ("CardExpYear", Cstr(request("expr_yyyy")))
	centinelRequest.Add ("OrderNumber", Cstr(request("order_number")))
	centinelRequest.Add ("OrderDescription", Cstr(request("order_description")))
	centinelRequest.Add ("UserAgent", Cstr(Request.ServerVariables("HTTP_USER_AGENT")))
	centinelRequest.Add ("BrowserHeader", Cstr(Request.ServerVariables("HTTP_ACCEPT")))
	centinelRequest.Add ("IPAddress", Cstr(Request.ServerVariables("REMOTE_ADDR")))
	centinelRequest.Add ("EMail", Cstr(request("email_address")))
	centinelRequest.Add ("BillingFirstName", Cstr(request("b_first_name")))
	centinelRequest.Add ("BillingMiddleName", Cstr(request("b_middle_name")))
	centinelRequest.Add ("BillingLastName", Cstr(request("b_last_name")))
	centinelRequest.Add ("BillingAddress1", Cstr(request("b_address1")))
	centinelRequest.Add ("BillingAddress2", Cstr(request("b_address2")))
	centinelRequest.Add ("BillingCity", Cstr(request("b_city")))
	centinelRequest.Add ("BillingState", Cstr(request("b_state")))
	centinelRequest.Add ("BillingCountryCode", Cstr(request("b_country_code")))
	centinelRequest.Add ("BillingPostalCode", Cstr(request("b_postal_code")))
	centinelRequest.Add ("BillingPhone", Cstr(request("b_phone")))
	centinelRequest.Add ("ShippingFirstName", Cstr(request("s_first_name")))
	centinelRequest.Add ("ShippingMiddleName", Cstr(request("s_middle_name")))
	centinelRequest.Add ("ShippingLastName", Cstr(request("s_last_name")))
	centinelRequest.Add ("ShippingAddress1", Cstr(request("s_address1")))
	centinelRequest.Add ("ShippingAddress2", Cstr(request("s_address2")))
	centinelRequest.Add ("ShippingCity", Cstr(request("s_city")))
	centinelRequest.Add ("ShippingState", Cstr(request("s_state")))
	centinelRequest.Add ("ShippingCountryCode", Cstr(request("s_country_code")))
	centinelRequest.Add ("ShippingPostalCode", Cstr(request("s_postal_code")))
	centinelRequest.Add ("ShippingPhone", Cstr(request("s_phone")))
	centinelRequest.Add ("TransactionAction", Cstr(request("payment_action")))
	centinelRequest.Add ("NoShipping", Cstr(request("no_shipping")))
	centinelRequest.Add ("OverrideAddress", Cstr(request("override_address")))
	centinelRequest.Add ("ForceAddress", Cstr(request("force_address")))
	centinelRequest.Add ("PromoCode", Cstr(request("promo_code")))

	
	'=====================================================================================
	' Send the XML Msg to the MAPS Server
	' SendHTTP will send the cmpi_lookup message to the MAPS Server (requires fully qualified URL)
	' The Response is the CentinelResponse Object
	'=====================================================================================
	
	Try
		centinelResponse = centinelRequest.sendHTTP(Cstr(TransactionURL), Cstr(Timeout))

	Catch ex As Exception

		strErrorNo =  "9040"
		strErrorDesc = "Communication Error"

		Session("Centinel_ErrorNo") = strErrorNo
		Session("Centinel_ErrorDesc") = strErrorDesc

	End Try

	If strErrorNo = "" Then
		
		strCardEnrolled = centinelResponse.getValue("Enrolled")		
		strErrorNo = centinelResponse.getValue("ErrorNo")			 
		strErrorDesc = centinelResponse.getValue("ErrorDesc")
		strTransactionId = centinelResponse.getValue("TransactionId")
		strECI = centinelResponse.getValue("EciFlag")
		
		Session("Centinel_TransactionId") = strTransactionId
		Session("Centinel_Enrolled") = strCardEnrolled
		Session("Centinel_ErrorNo") = strErrorNo
		Session("Centinel_ErrorDesc") = strErrorDesc
		Session("Centinel_ECI") = strECI
		Session("Centinel_ACSURL") = centinelResponse.getValue("ACSUrl")
		Session("Centinel_PAYLOAD") = centinelResponse.getValue("Payload")

		If (strTransactionType = "C") Then
			Session("Centinel_PIType") = determineCardType(request("card_number"))
		End IF

		Session("Centinel_TransactionType") = strTransactionType

	End If

	'======================================================================================
	' Determine how to proceed with the transaction. Note that this example handles
	' PayPal payments, Payer Authentication and SECURE-eBill Processing
	'======================================================================================

	
	If UCase(strTransactionType) = "P" or UCase(strTransactionType) = "X" or UCase(strTransactionType) = "A" Then
		
	'======================================================================================
	' Handle PayPal Logic
	'======================================================================================

			Session("CENTINEL_ISPAYPAL") = "Y"

			If strErrorNo = "0" AND strCardEnrolled = "Y" Then 

				Response.redirect ("ccTransfer.aspx")

			ElseIf strErrorNo = "0" AND strCardEnrolled = "U" Then

				'======================================================================================
				' Unable to process PayPal payment at this time.
				'======================================================================================

				Session("Message") = Session("Centinel_ErrorNo") + "PayPal payment was unable to complete. Please provide another form of payment to complete your transaction."
				Response.redirect ("ccLookup.aspx")

			Else 
					
				'======================================================================================
				' Log Error Message, this is an unexpected state
				'======================================================================================

				Session("Message") = Session("Centinel_ErrorNo") + "PayPal payment was unable to complete. Please provide another form of payment to complete your transaction."
				Response.redirect ("ccLookup.aspx")

			End If

	ElseIf UCase(strTransactionType) = "E" Then
	
	'======================================================================================
	' Handle SECURE-eBill Logic
	'======================================================================================

		If strErrorNo = "0" AND strCardEnrolled = "Y" Then 

			'======================================================================================
			' Proceed to SECURE-eBill Transaction
			'======================================================================================

			Response.redirect ("frames/ccFrame.aspx")

		ElseIf strErrorNo = "0" AND strCardEnrolled = "U" Then

			'======================================================================================
			' SECURE-eBill Payments are Not Available
			' Prompt for another form of payment
			'======================================================================================
				
			Session("Message") = "Secure-eBill Payment was unable to complete. Please provide another form of payment to complete your transaction."
			Response.redirect ("ccLookup.aspx")
				

		Else 
					
			'==================================================================================
			' An error was encountered
			' Log Error Message, this is an unexpected state
			' Proceed to authorization to complete the transaction.
			'==================================================================================

			Response.redirect ("ccResults.aspx")

		End If

	ElseIf UCase(strTransactionType) = "B" Then
	'======================================================================================
	' Handle Bill Me Later Logic
	'======================================================================================

		If strErrorNo = "0" AND strCardEnrolled = "Y" Then 

			'======================================================================================
			' Proceed to Bill Me Later Transaction
			'======================================================================================

			Response.redirect ("frames/ccFrame.aspx")

		ElseIf strErrorNo = "0" AND strCardEnrolled = "U" Then

			'======================================================================================
			' Bill Me Later Payments are Not Available
			' Prompt for another form of payment
			'======================================================================================
				
			Session("Message") = "Bill Me Later Payment was unable to complete. Please provide another form of payment to complete your transaction."
			Response.redirect ("ccLookup.aspx")
				

		Else 
					
			'==================================================================================
			' An error was encountered
			' Log Error Message, this is an unexpected state
			' Proceed to authorization to complete the transaction.
			'==================================================================================

			Response.redirect ("ccResults.aspx")

		End If


	Else

	'======================================================================================
	' Handle Payer Authentication Logic
	'======================================================================================

		If strErrorNo = "0" AND strCardEnrolled = "Y" Then 

			'======================================================================================
			' Proceed to Payer Authentication
			'======================================================================================

			Response.redirect ("frames/ccFrame.aspx")

		ElseIf strErrorNo = "0" AND strCardEnrolled = "U" Then

			'======================================================================================
			' Proceed to Authorization, Payer Authentication Not Available
			' Set the proper ECI value based on the Card Type
			'======================================================================================

			Session("Message") = "Your Transaction has completed successfully."
			Response.redirect ("ccResults.aspx")


		ElseIf strErrorNo = "0" AND strCardEnrolled = "N" Then

			'======================================================================================
			' Proceed to Authorization, Payer Authentication Not Available
			' The ECI Value for Authorization is returned on the Lookup Response
			'======================================================================================

			Response.redirect ("ccResults.aspx")
					
		Else 
					
			'==================================================================================
			' An error was encountered
			' Log Error Message, this is an unexpected state
			' Proceed to authorization to complete the transaction.
			'==================================================================================

			Response.redirect ("ccResults.aspx")

		End If

	End If
	

	centinelResponse = nothing
	centinelRequest = nothing
%>
