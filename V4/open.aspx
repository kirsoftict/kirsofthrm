<%@ Page Language="VB" AutoEventWireup="false" CodeFile="open.aspx.vb" Inherits="scripts_open" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="Microsoft.VisualBasic" %>
<%@ import Namespace="System.IO" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
<script language="javascript" type="text/javascript" src="jq/jquery-1.7.2.js"></script>

</head>
<body>
    <script type="text/javascript">
 //  alert("it is goona process");
    </script>
    <div>
  <%  
      Dim sc As New k_security
      Dim db As New dbclass
      Dim sql As String = ""
      Dim flg As Integer = 0
      Dim flg2 As Integer = 0
      ' Response.Write(sc.d_encryption("zewde@123"))
      Dim rd As String = ""
      
      Dim tbl As String = ""
      Dim key As String = ""
      Dim keyval As String = ""
      If Request.QueryString.HasKeys = True Then
          tbl = Request.QueryString("tbl")
          key = Request.QueryString("key")
          rd = Request.QueryString("rd")
          keyval = Request.QueryString("keyval")
          If Request.QueryString("task") = "update" Then
              Response.Write("<script type='text/javascript'>alert('updating....');</script>")
              sql = db.makeupdate_statement(tbl, Request.QueryString, session("con"), key, keyval)
              flg = db.save(sql, session("con"),session("path"))
              'Response.Write(sql)
              If flg = 1 Then
                  Response.Write("<span style='font-size:15pt; color:Green;'> Data Updated </span>")
              End If
          Else
              Response.Write("<script type='text/javascript'>//alert('saving....');</script>")

              sql = db.makest(tbl, Request.QueryString, session("con"), key)
              ' Response.Write(sql)
              flg = db.save(sql, session("con"),session("path"))
              If flg = 1 Then
                  If Request.QueryString("emp_id") <> "" Then
                      Session("emp_id") = Request.QueryString("emp_id")
                  End If
                  Response.Write("<span style='font-size:15pt; color:Green;'> Data Saved </span>")
              End If
              End If
              'MsgBox(rd)
         
              ' sql = db.makest(tbl, Request.QueryString, session("con"), key)
      End If
      If flg <> 1 Then
          Response.Write("<span style='font-size:15pt; color:Red;'>Sorry Data doesnt change </span>")
      End If
      %>
    </div>
    <form id="frmx" action="" method="post">
    <input type="hidden" id='datatake' name='datatake' />
    </form>
    <% If flg = 1 Then
            Response.Write("date is saved")
            %>
    
    <script type="text/javascript">
       $("#datatake").val("<% response.write(session("emp_id")) %>");
        $('#frmx').attr("target","frm_tar");
        $('#frmx').attr("action","<% response.write(rd) %>");
        $('#frmx').submit();
    </script>
   <% end if %>
   <script type="text/javascript" language="ecmascript">
   window.close();
   </script>
</body>
</html>
