<% @Page Language="VB" Explicit="True" aspcompat="true" %>
<%
'===========================================================================
'=  CardinalCommerce (http://www.cardinalcommerce.com)
'=  ccNotification.aspx
'=  Version 6.0.0 03/01/2006
'=
'=	This page receives PayPal payment update notifications from Centinel via
'=	HTTP posts and queries Centinel for the PayPal payment status update.
'=
'=	Merchants should add functionality to handle PayPal payment status
'=	changes in the locations marked "'// ADD LOGIC" below.
'===========================================================================
%>
<!-- #Include File="CentinelConfig.aspx"-->
<%@ import Namespace="CardinalCommerce" %>

<%

    dim notificationId, reqProcessorId, reqMerchantId, strErrorNo, strErrorDesc
    dim errorNo, errorDesc, transactionId, transactionType, status, signatureStatus, purchaseAmount, rawAmount, isoCurrencyNumber
    dim orderNumber, orderDesc, recurring, recurringFrequency, installment, personFirstName, personLastName, address1, address2
    dim city, stateProvince, postalCode, countryCode, email


    notificationId = Request("NotificationId")
    reqProcessorId = Request("ProcessorId")
    reqMerchantId = Request("MerchantId")

	' IIS Logging
	' Response.AppendToLog "PayPal Notification Received: " & notificationId

        if len(notificationId) > 0 then

		Dim centinelRequest As New CentinelRequest()
		Dim centinelResponse As New CentinelResponse()

		'======================================================================================
		' Construct Message using Name / Value pairs
		'======================================================================================

		centinelRequest.Add ("MsgType", "cmpi_paypal_status")
		centinelRequest.Add ("Version", Cstr(MessageVersion))
		centinelRequest.Add ("ProcessorId", Cstr(ProcessorId))
		centinelRequest.Add ("MerchantId", Cstr(MerchantId))
		centinelRequest.Add ("TransactionPwd", Cstr(TransactionPwd))
		centinelRequest.Add ("NotificationId", Cstr(notificationId))
        

		'=====================================================================================
		' Send the XML Msg to the MAPS Server
		' SendHTTP will send the cmpi_lookup message to the MAPS Server (requires fully qualified URL)
		' The Response is the CentinelResponse Object
		'=====================================================================================
		
		Try
			centinelResponse = centinelRequest.sendHTTP(Cstr(TransactionURL), Cstr(Timeout))
		
		Catch ex As Exception
           
			strErrorNo = "9040"
			strErrorDesc = "Communication Error"
		End Try

		If strErrorNo = "" Then
		
			' IIS Logging
			' Response.AppendToLog "cmpi_paypal_status response: " & centinelResponse.GetUnparsedResponse()

			
			errorNo = centinelResponse.getValue("ErrorNo")			 
			errorDesc = centinelResponse.getValue("ErrorDesc")
			transactionId = centinelResponse.getValue("TransactionId")
			transactionType = centinelResponse.getValue("TransactionType")
			status = centinelResponse.getValue("PayPalStatus")
			signatureStatus = centinelResponse.getValue("SignatureVerification")
			purchaseAmount = centinelResponse.getValue("PurchaseAmount")
			rawAmount = centinelResponse.getValue("RawAmount")
			isoCurrencyNumber = centinelResponse.getValue("PurchaseCurrency")
			orderNumber = centinelResponse.getValue("OrderNumber")
			orderDesc = centinelResponse.getValue("OrderDesc")
			recurring = centinelResponse.getValue("Recurring")
			recurringFrequency = centinelResponse.getValue("RecurringFrequency")
			installment = centinelResponse.getValue("Installment")
			personFirstName = centinelResponse.getValue("FirstName")
			personLastName = centinelResponse.getValue("LastName")
			address1 = centinelResponse.getValue("Address1")
			address2 = centinelResponse.getValue("Address2")
			city = centinelResponse.getValue("City")
			stateProvince = centinelResponse.getValue("State")
			postalCode = centinelResponse.getValue("PostalCode")
			countryCode = centinelResponse.getValue("CountryCode")
			email = centinelResponse.getValue("EMail")
		
		End If

		dim iStatus, iTxnType
		iStatus = cInt(status)
		iTxnType = cInt(transactionType)

		centinelResponse = nothing
		centinelRequest = nothing

        ' With the information provided, update the status of the order in your system.

        if iTxnType = 1 then
            ' Payment

            '// ADD LOGIC
            '// - retrieve the order from your system

        elseif iTxnType = 2 then
            ' PreApproved Payment

            '// ADD LOGIC
            '// - do nothing or mark the order as pending in your system

        elseif iTxnType = 3 then
            ' PreApproved Payment Agreement

            '// ADD LOGIC
            '// - retrieve the order from your system

	elseif iTxnType = 4 then
            ' Express Checkout

            '// ADD LOGIC
            '// - retrieve the order from your system
	 

	elseif iTxnType = 5 then
            ' Express Checkout ReAuthorization

            '// ADD LOGIC
            '// - retrieve the order from your system

	elseif iTxnType = 6 then
            ' Express Checkout Authorization

            '// ADD LOGIC
            '// - retrieve the order from your system

	elseif iTxnType = 7 then
            ' Express Checkout Sale

            '// ADD LOGIC
            '// - retrieve the order from your system

	elseif iTxnType = 8 then
            ' Express Checkout Order

            '// ADD LOGIC
            '// - retrieve the order from your system


	elseif iTxnType = 10 then
            ' Batch Settlement Credit

            '// ADD LOGIC
            '// - retrieve the order from your system

	elseif iTxnType = 11 then
            ' Batch Settlement Debit

            '// ADD LOGIC
            '// - retrieve the order from your system
	 

	elseif iTxnType = 12 then
            ' Express Checkout Capture

            '// ADD LOGIC
            '// - retrieve the order from your system

	elseif iTxnType = 13 then
            ' Express Checkout Refund

            '// ADD LOGIC
            '// - retrieve the order from your system

	elseif iTxnType = 14 then
            ' Express Checkout Void

            '// ADD LOGIC
            '// - retrieve the order from your system

	elseif iTxnType = 15 then
            ' Direct Payment Authorization

            '// ADD LOGIC
            '// - retrieve the order from your system


	elseif iTxnType = 16 then
            ' Direct Payment Sale

            '// ADD LOGIC
            '// - retrieve the order from your system

	elseif iTxnType = 17 then
            ' Direct Payment Capture

            '// ADD LOGIC
            '// - retrieve the order from your system
	 

	elseif iTxnType = 18 then
            ' Direct Payment Void

            '// ADD LOGIC
            '// - retrieve the order from your system

	elseif iTxnType = 19 then
            ' Direct Payment Refund

            '// ADD LOGIC
            '// - retrieve the order from your system

	elseif iTxnType = 20 then
            ' Direct Payment Re-Authorization

            '// ADD LOGIC
            '// - retrieve the order from your system

	elseif iTxnType = 25 then
            ' Payment Refund

            '// ADD LOGIC
            '// - retrieve the order from your system


	elseif iTxnType = 26 then
            ' PreApproved Payment Refund

            '// ADD LOGIC
            '// - retrieve the order from your system
	
	elseif iTxnType = 27 then
            ' PreApproved Agreement Cancelation 

            '// ADD LOGIC
            '// - retrieve the order from your system
	 

        else
            '// ADD LOGIC
            '// - log an error, this should not be received

        end if
        

        
    else
		
	' IIS Logging
        ' Response.AppendToLog "Centinel Notification POST Error MISSING PARAMETER notification_id"

    end if
%>
