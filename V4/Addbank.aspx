<%@ Page Language="VB" AutoEventWireup="false" CodeFile="addbank.aspx.vb" Inherits="addbank" %>
<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<% 
    lineone()
   %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title></title>

<script language="javascript" type="text/javascript" src="jq/jquery-1.7.2.js"></script>
<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />

<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/script.js" type="text/javascript"></script>
<script type="text/javascript" src="jq/ui/jquery.ui.core.js"></script>
<script type="text/javascript" src="jq/ui/jquery.ui.widget.js"></script>
	<script type="text/javascript"  src="jq/ui/jquery.ui.mouse.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.draggable.js"></script>
	<script type="text/javascript" src="scripts/form.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.position.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.resizable.js"></script>
	<!--script type="text/javascript" src="jq/ui/jquery.ui.button.js"></script--><script type="text/javascript" src="jq/ui/jquery.ui.dialog.js"></script>
	
<script type="text/javascript">
    var prv;
    prv = "";
    var id;
    var focused = "";
    var requf = ["id", "bank_name", "x"];
    var fieldlist = ["bank_name", "abr", "who_reg", "date_reg","XX"];
    function validation1() {
        var str1 = $("#frmbankname").formSerialize();
      //  alert(str1);
        if ($('#bank_name').val() == '') { showMessage('Bank Name cannot be empty', 'bank_name'); $('#bank_name').focus(); return false; }
        if ($('#abr').val() == '') { showMessage('abr cannot be empty', 'abr'); $('#abr').focus(); return false; }
      
        else if (focused == "") {
            var ans;
            ans = checkblur();
            if (ans != true) {
                $("#" + ans).focus();
            } else {
                var str = $("#frmbankname").formSerialize();
                $("#frmbankname").attr("action", "?tbl=tblbanks&task=<% response.write(keyp) %>&lrd=empcontener&key=id&keyval=<% response.write(idx) %>&rd=empcontener.aspx&" + str);
                $("#frmbankname").submit();
                return true;
            }
        }
    } 


</script>

</head>

<body style="height:auto;">
<div></div>

    <% If Session("username") = "" Then
       %>
       <script type="text/javascript">
           //document.location.href="admin_home.php"
           window.location = "logout.aspx";
</script>
       <%
       End If
       
       If Session("emp_iid") = "" Then
       %>

<script type="text/javascript">
    //document.location.href="admin_home.php"
    window.location = "logout.aspx?msg=session expired";
</script>

<%
Else
     %>

<script type="text/javascript">
    //alert('<% response.write(session("bank_name")) %>');
</script>

<%    
End If
'Response.Write(Session("fullempname"))

Dim sc As New k_security
' Response.Write(sc.d_encryption("zewde@123"))
If Request.Form.HasKeys = True Then
    'Dim db As New dbclass
    ' Dim sql As String
    ' sql = db.makest("tblworkexp", Request.Form, session("con"), "")
    'Response.Write(sql)
    'db.save(sql, session("con"),session("path"))
End If
For Each p As String In Request.Form
    'Response.Write(" <br />" & p & "=>" & Request.Form(p))
 
Next
For Each k As String In Request.ServerVariables
    ' Response.Write("<br />" & k & "=>" & Request.ServerVariables(k))
Next
'Response.Write("<br />" & Request.Form("do"))
'Dim fm As New formMaker
 %>

 <div id="formouterbox_small" style="max-width:550px;">
     <div id="formheader">
    <span class="titlet">
This is Title</span>
<span class="close" id="clickclose_s" style="cursor:pointer;"></span>
        <div class="head1">&nbsp;</div><div class="head2">&nbsp;</div><div class="head3">&nbsp;</div>
        </div>
    <div id="forminner">
    <span id="messageboxx"></span>
  <form method='post' id='frmbankname' name='frmbankname' action=""> 
<table><tr><td>Bank Name</td><td>:</td><td> <input type='text' id='bank_name' name='bank_name' value='<%response.write(session("bank_name")) %>' /></td></tr><tr><td>
abr<sup style='color:red;'>*</sup></td>
<td>:</td><td><input type="text"  id='abr' name='abr' /><label class='lblsmall'>CBE,DASHEN,AWASH</label></td>
</tr>
<tr><td>
Bank Contact<sup style='color:red;'>*</sup></td>
<td>:</td><td><input type="text"  id='bankcontact' name='bankContact' /><label class='lblsmall'></label></td>
</tr><tr><td><input type='hidden' id='who_reg' name='who_reg' value="<%response.write(session("username")) %>" />
<input type='hidden' id='date_reg' name='date_reg' value="<% dim lucur(3) as string
 lucur(2) = Today.Year.ToString
     lucur(1) = Today.Month.ToString
     lucur(0) = Today.Day.ToString
 dim sdate as string=lucur(1) & "/" & lucur(0) & "/" & lucur(2)
 response.write(sdate) %>" /></td><td>:</td><td><br /><label class='lblsmall'></label></td>
</tr><tr><td colspan='4'><input type='button' name='btnSave' id='btnSave' value='Save' />
<input type='button' onclick="javascript:window.location='addbank.aspx'" name='reset' value='Reset' /></td></tr></table></form>
    </div>
    <sup style="color:Red;">*</sup>Required Fields
 </div>
    
 <div id="listx" style=" width:100%; padding:0px 7px 0px 0px;">
    <%  Dim db As New dbclass
        Dim dt As DataTableReader
        If keyp = "update" Then
            Dim val As String = ""
            dt = db.dtmake("new" & Today.ToLocalTime, "select * from tblbanks where id=" & Request.QueryString("id"), Session("con"))
      If dt.HasRows = True Then
          dt.Read()
          Response.Write("<script type='text/javascript'>")
                For k As Integer = 0 To dt.FieldCount - 4
                    val = ""
                    val = dt.Item(k).ToString.Trim
                    Dim val2 As String = ""
                    For Each c As Char In val
                        If Asc(c) = 10 Then
                            val2 = val2 & "\n"
                        Else
                            val2 = val2 & c
                        End If
                        'Response.Write(Asc(c) & " ::")
                    Next
                    'Response.Write("//" & dt.GetDataTypeName(k).ToLower)
                    If LCase(dt.GetDataTypeName(k)) = "string" And dt.IsDBNull(k) = False Then
                     %>
                      $("#<%response.write(dt.getname(k)) %>").val("<% response.write(val2) %>");
                    <% 
                    ElseIf LCase(dt.GetDataTypeName(k)) = "datetime" And dt.IsDBNull(k) = False Then
                        Dim sdatex As Date = dt.Item(k)
                        Dim d As String = sdatex.ToShortDateString
                        Dim da As String = sdatex.Day
                        Dim mm As String = sdatex.Month
                        Dim yy As String = sdatex.Year
                        d = mm & "/" & da & "/" & yy
                        Response.Write("$('#" & dt.GetName(k) & "').val('" & d & "');")
                    Else
                        
                         %>
                       
                        $("#<%response.write(dt.getname(k)) %>").val("<% response.write(val2) %>");
                    <%
                    End If
                   
                Next
                    Response.Write("$('#btnSave').attr('title','update');$('#btnSave').attr('value','Update');</script>")
                End If
                dt.Close()
            End If
            db = Nothing
            dt = Nothing
        Dim mk As New formMaker
            Dim row As String
            Dim loc As String
            loc = Request.ServerVariables("url").Substring(1, Request.ServerVariables("url").Length - 1)
        Dim sqlx As String = "select id,bank_name,abr from tblbanks order by id desc"
        row = mk.edit_del_list("tblbanks", sqlx, "Bank Name,Abr", Session("con"), loc)
            Response.Write(row)
        
    %>
 </div>
<div id="newpage"></div>
<script type="text/javascript" language="javascript">
    hform = findh(document.getElementById("middle_bar"));
    $('.titlet').text("Add Bank");
    //showobjar("formx","titlet",22,2);
   
  </script>
  
    <script type="text/javascript">
        function del(val, ans, hd) {

            if (ans == "yes") {
                // alert(val + ans);
                $('#frmx').attr("target", "_self");
                $('#frmx').attr("action", "<% response.write(loc) %>?task=delete&id=" + val + "&tbl=tblbanks");
                $('#frmx').submit();
            }
            else {
                ha(hd);
            }
        }
   </script>
    <form id="frmx" action="" method="post">
    </form>
   
   
   <%  ' Response.Write(keyp)
       If keyp = "delete" Then
           Dim fs As New file_list
           Dim con As String
           Dim str As String
           con = "<span style='color:red;'> This row of data will not be come again.<br />Are you sure you want delete it?<br /><hr>" & _
           "<img src='images/gif/btn_delete.gif' style='cursor:pointer;' onclick=" & Chr(34) & "javascript:del('" & idx & "','yes','del123');alert('" & idx & "');" & Chr(34) & "></span>"
           fs.msgboxt("del123", "Caution! Deleting", con)
           str = "<div id='del123' style=" & Chr(34) & "opacity:0.9;filter:alpha(opacity=90); background:#9fdfaf; left:400px; top:200px; width:600px; height:400px; text-align:center; vertical-align:middle; position:absolute; content:open-quote;" & Chr(34) & _
            "><div style=" & Chr(34) & "height:30px; background:url(images/blue_banner-760x147.jpg); vertical-align:top;" & Chr(34) & _
            "><div style=" & Chr(34) & "text-align:left; font-size:16px; color:#000099; width:120px; position:absolute; left:2px;" & Chr(34) & " dir=" & Chr(34) & "ltr" & Chr(34) & _
            "><b>Warrening</b></div><div style=" & Chr(34) & "cursor:pointer; text-align:right; height:30px; width:22px; color:#CC0000; background:url(../images/xp.gif); background-repeat:no-repeat; right:0px; position:absolute" & Chr(34) & " dir=" & Chr(34) & "rtl" & Chr(34) & " onClick=" & Chr(34) & "javascript: document.getElementById('" & CStr(ID) & "').style.visibility='hidden';" & Chr(34) & _
            ">&nbsp; </div></div><br /><br />" & _
       "<div align=" & Chr(34) & "center" & Chr(34) & " style=" & Chr(34) & "width:100%; height:300px; overflow:scroll; font-size:12px; color:blue;" & Chr(34) & _
       ">&nbsp;&nbsp;" & CStr(con) & "</div></div>"
           ' Response.Write(str)
           %> 
           <div id="dialog-modal" title="Caution"><% response.write(con) %></div>
           <script type="text/javascript">

               //$( "#dialog:ui-dialog" ).dialog( "destroy" );

               $("#dialog-modal").dialog({
                   resizable: true,
                   modal: true
               });
           </script>
           <%
           
       End If
           If IsNumeric(flg) = True Then%>
    <script type="text/javascript">
        //$(document).delay(80000);
        $('#frmx').attr("target", "_parent");

        //$('#frmx').attr("action","<% response.write(rd) %>");
        // $('#frmx').submit();
    </script>
   <%  End If%>
   
   
</body>
</html>
  <script src="scripts/kirsoft.required.js" type="text/javascript"></script>


