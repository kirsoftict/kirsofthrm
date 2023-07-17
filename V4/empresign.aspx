<%@ Page Language="VB" AutoEventWireup="false" CodeFile="empresign.aspx.vb" Inherits="empresign" %>

<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import Namespace="system.net" %>
<%@ Import Namespace="system.IO" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%  Dim client As New WebClient()
    If String.IsNullOrEmpty(Session("emptid")) = True Then
        Response.Redirect("logout.aspx")
    End If
    Dim keyp As String = ""
    Dim addbutton As Boolean = True
    If Request.QueryString("dox") = "edit" Then
        keyp = "update"
    ElseIf Request.QueryString("dox") = "delete" Then
        keyp = "delete"
    Else
        keyp = "save"
    End If
    Dim idx As String = ""
    idx = Request.QueryString("id")
    Dim msg As String = ""
    Dim dbx As New dbclass
      Dim sql As String = ""
      Dim flg As Integer = 0
    Dim flg2 As Integer = 0
    Dim fm As New formMaker
     Dim flg1 As Object
      ' Response.Write(sc.d_encryption("zewde@123"))
      Dim rd As String = ""
     
      Dim tbl As String = ""
      Dim key As String = ""
        Dim keyval As String = ""
        tbl = Request.QueryString("tbl")
        key = Request.QueryString("key")
        rd = Request.QueryString("rd")
        If Request.QueryString.HasKeys = True Then
            If Request.QueryString("dox") = "" Then
            keyval = Request.QueryString("keyval")
            Dim emp_id As String = fm.getinfo2("select emp_id from emprec where id=" & Session("emptid"), Session("con"))
           
                If Request.QueryString("task") = "update" Then
                ' Response.Write("<script type='text/javascript'>alert('updating....');</script>")
                    sql = dbx.makeupdate_statement(tbl, Request.QueryString, session("con"), key, keyval)
                flg = dbx.excutes(sql, Session("con"), Session("path"))
                    ' Response.Write(sql)
                    If flg = 1 Then
                    msg = "Data Updated"
                    client.DownloadFile(Session("http_host") & "/" & "reportnonactive.aspx?val=all", Session("path") & "\reportnon.html")
                    End If
                ElseIf Request.QueryString("task") = "delete" Then
                'Response.Write("<script type='text/javascript'>alert('deleting....');</script>")
                    ' sql = dbx.makeupdate_statement(tbl, Request.QueryString, session("con"), key, keyval)
                sql = "delete from " & Request.QueryString("tbl") & " where id=" & Request.QueryString("id") & Chr(13) '
                Dim dr As Date
                dr = fm.getinfo2("select resign_date from " & Request.QueryString("tbl") & " where id=" & Request.QueryString("id"), Session("con"))
                sql &= "update emprec set end_date=Null,active='y' where id=" & Session("emptid") & " and end_date='" & dr & "'" & Chr(13)
                sql &= "update emp_job_assign set date_end=Null where emptid=" & Session("emptid") & " and date_end='" & dr & "'" & Chr(13)
                sql &= "update emp_sal_info set date_end=Null where emptid=" & Session("emptid") & " and date_end='" & dr & "'" & Chr(13)
                sql &= "update emp_pardime set to_date=Null where emptid=" & Session("emptid") & " and to_date='" & dr & "'" & Chr(13)
                sql &= "update emp_contract set status=Null where emptid=" & Session("emptid") & " and dateend='" & dr & "'" & Chr(13)
                sql &= "update emp_alloance_rec set active='y',to_date=Null where emptid=" & Session("emptid") & " and to_date='" & dr & "'" & Chr(13)
                sql &= "update login set active='y' where emp_id='" & emp_id & "'" & Chr(13)
            
                Dim rs As DataTableReader
                rs = dbx.dtmake("xclose", "select id from emp_alloance_rec where emptid=" & Session("emptid") & " and to_date='" & dr & "'", Session("con"))
                If rs.HasRows Then
                    While rs.Read
                        sql &= "Update emp_alloance_rec set to_date=Null where id=" & rs.Item("id") & Chr(13)
                    End While
                End If
               
                rs.Close()
                sql = "BEGIN TRANSACTION " & Session("emp_iid") & Chr(13) & sql
                flg1 = dbx.excutes(sql, Session("con"),session("path"))
                Response.Write(flg1.ToString)
                If IsNumeric(flg1) Then
                    If CInt(flg1) > 0 Then
                        flg1 = dbx.excutes("Commit Transaction " & Session("emp_iid"), Session("con"), Session("path"))
                        If CStr(flg1) <> "-1" Then
                            dbx.excutes("RollBack transaction " & Session("emp_iid"), Session("con"), Session("path"))
                            Response.Write("<span style='font-size:15pt; color:Red;'>Data is Rollbacked</span>")

                        Else
                            Response.Write("<span style='font-size:15pt; color:Red;'>Data is Deleted  </span>")
                            client.DownloadFile(Session("http_host") & "/" & "reportnonactive.aspx?val=all", Session("path") & "\reportnon.html")
                            client.DownloadFile(Session("http_host") & "/" & "viewreport1.aspx?val=all&active=y", Session("path") & "\reportactive.html")

                        End If
                    End If
                End If
                'Response.Write("<textarea cols=100 rows=5>" & sql & "</textarea>")
                ' flg = dbx.save(sql, session("con"),session("path"))
                   
                    ' Response.Write(sql)
                If flg1 = -1 Then
                    'msg = "Data deleted"
                End If
                ElseIf Request.QueryString("task") = "save" Then
                ' Response.Write("<script type='text/javascript'>alert('saving....');</script>")
               
                sql = dbx.makest(tbl, Request.QueryString, Session("con"), key) & Chr(13)
                    ' Response.Write(sql)
                'flg = dbx.save(sql, session("con"),session("path"))
                   
                ' msg = "Data Saved"
                    sql &= "update emprec set end_date='" & Request.QueryString("resign_date") & "',active='n' where id=" & Session("emptid") & Chr(13)
                sql &= "update emp_job_assign set date_end='" & Request.QueryString("resign_date") & "' where emptid=" & Session("emptid") & " and date_end is null" & Chr(13)
                sql &= "update emp_sal_info set date_end='" & Request.QueryString("resign_date") & "' where emptid=" & Session("emptid") & " and date_end is null" & Chr(13)
                sql &= "update emp_pardime set to_date='" & Request.QueryString("resign_date") & "' where emptid=" & Session("emptid") & " and to_date is null" & Chr(13)
                sql &= "update login set active='n' where emp_id='" & emp_id & "'" & Chr(13)
           
                '   sql &= "update emp_alloance_rec set to_date='" & Request.QueryString("resign_date") & "' where emptid=" & Session("emptid") & " and to_date is null" & Chr(13)
           
                Dim rs As DataTableReader
                rs = dbx.dtmake("xclose", "select id from emp_alloance_rec where emptid=" & Session("emptid") & " and to_date is NULL", Session("con"))
                If rs.HasRows Then
                    While rs.Read
                        sql &= "Update emp_alloance_rec set to_date='" & Request.QueryString("resign_date") & "' where id=" & rs.Item("id") & Chr(13)
                    End While
                End If
               
                rs.Close()
                sql = "BEGIN TRANSACTION " & Session("emp_iid") & Chr(13) & sql
                flg1 = dbx.excutes(sql, Session("con"),session("path"))
                'Response.Write(flg1.ToString)
                If IsNumeric(flg1) Then
                    If CInt(flg1) > 0 Then
                        flg1 = dbx.excutes("Commit " & Session("emp_iid"), Session("con"), Session("path"))
                        If CStr(flg1) <> "-1" Then
                            dbx.excutes("RollBack " & Session("emp_iid"), Session("con"), Session("path"))
                            Response.Write("<span style='font-size:15pt; color:Red;'>Data is Rollbacked</span>")

                        Else
                            Response.Write("<span style='font-size:15pt; color:Red;'>Data is Saved and Updated  </span>")
                            client.DownloadFile(Session("http_host") & "/" & "reportnonactive.aspx?val=all", Session("path") & "\reportnon.html")
                            client.DownloadFile(Session("http_host") & "/" & "viewreport1.aspx?val=all&active=y", Session("path") & "\reportactive.html")
                        End If
                    End If
                End If
                Response.Write("<textarea cols=100 rows=5>" & sql & "</textarea>")
               
          
   
            End If
        End If
    End If

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
	<script type="text/javascript" src="jq/ui/jquery.ui.datepicker.js"></script>
	<!--script type="text/javascript" src="jq/ui/jquery.ui.button.js"></script--><script type="text/javascript" src="jq/ui/jquery.ui.dialog.js"></script>
<script type="text/javascript">
	$(function() {
		$( "#hire_date" ).datepicker({
			defaultDate: "+1w",
			changeMonth: true,
			changeYear:true,
			numberOfMonths: 2,
			minDate: "-70Y", maxDate: "-1d",
			onSelect: function( selectedDate ) {
				$( "#end_date" ).datepicker( "option", "minDate", selectedDate );
				
			}
		});
		$( "#end_date" ).datepicker( "option","dateFormat","mm/dd/yy");
		$( "#hire_date" ).datepicker( "option","dateFormat","mm/dd/yy");
		$( "#end_date" ).datepicker({
			defaultDate: "+1W",
			changeMonth: true,
			changeYear:true,
			numberOfMonths: 2,
			maxDate: "+1Y",
			onSelect: function( selectedDate ) {
				$( "#hire_date" ).datepicker( "option", "MaxDate", selectedDate );
			}
		});
	});
	</script>
<script type="text/javascript">

var prv;
  prv="";
var id;
var focused="";
var requf=["emp_id","emptid","reason","resign_date","x"];
var fieldlist=["emp_id","emptid","reason","resign_date","approved_by","approved_date","who_reg","date_reg","active","x"];
function validation1(){if ($('#emp_id').val() == '') {showMessage('emp_id cannot be empty','emp_id');$('#emp_id').focus();return false;}
if ($('#emptid').val() == '') {showMessage('emptid cannot be empty','emptid');$('#emptid').focus();return false;}
if ($('#reason').val() == '') {showMessage('reason cannot be empty','reason');$('#reason').focus();return false;}
if ($('#resign_date').val() == '') {showMessage('resign_date cannot be empty','resign_date');$('#resign_date').focus();return false;}
else if(focused=="") { var ans;
ans=checkblur();
if(ans!=true){ 
 $("#" + ans).focus();
}else{
    if(confirm("Are you sure this employee is going to Terminate?"))
    {
        var str=$("#frmemp_resign").formSerialize();
        $("#frmemp_resign").attr("action","?tbl=emp_resign&task=<% response.write(keyp) %>&lrd=empcontener&key=id&keyval=<% response.write(idx) %>&rd=empcontener.aspx&" + str);
        $("#frmemp_resign").submit();
        return true;
    }
  }
  }
} 
function checkchk(id)
{
    
    if($("#" +id).is(':checked'))
    {
       $('#holiday').val($("#" +id).val() +","+ $('#holiday').val() );
      // alert($('#holiday').val());
    }
    else
    {
        var cv=$("#" +id).val();
        var n=$('#holiday').val();
        var nn=n.split(",");
        //alert(nn.length);
        var i;
         $('#holiday').val("");
        for(i=0;i<nn.length;i++)
        {
        //
            if(cv!=nn[i] && nn[i]!="")
            { alert(cv +"="+nn[i]);
             $('#holiday').val(nn[i]+","+$('#holiday').val()  );
            }
        }
        //alert($('#holiday').val());
       // alert(cv);
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
window.location="logout.aspx";
</script>
       <%
       End If
       
       If Session("emp_id") = "" Then
       %>

<script type="text/javascript">
 //document.location.href="admin_home.php"
window.location="empcontener.aspx";
</script>

<%
Else
     %>

<script type="text/javascript">
//alert('<% response.write(session("emp_id")) %>');
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
'Response.Write(Session("emptid"))
If Session("con").state = Data.ConnectionState.Closed Then
   ' Response.Write("database is closed")
    Session("con").open()
End If
sql = "select * from emp_resign where emptid='" & Session("emptid") & "' order by id desc"
Dim dtt As DataTableReader
dtt = dbx.dtmake("onerect", sql, session("con"))
If dtt.HasRows = True Then
    addbutton = False
    msg = "Add Button is Disabled this person contract ended maintain the hiring information"
Else
    
    msg = "Add New Data"
End If
dtt.Close()

 %>
 <div style="top:0px;height:45px; width:80px; background:url(images/gif/add_new.gif) no-repeat; cursor:pointer" id="addbutton" onclick="javascript: $('#frmx').attr('target','_self'); $('#frmx').attr('action','<% response.write(Request.ServerVariables("URL")) %>?task=addnew'); $('#frmx').submit();">
     </div>
     <div id="outermssage" style="width:75%; height:25px; display:none; color:Green; background:url(images/gif/warn16_1.gif) no-repeat; text-align:center; font-weight:bold; font-size:12pt;"></div>
 <div id="formouterbox_small" style="display:none;">
     <div id="formheader">
    <span class="titlet">
This is Title</span>
<span class="close" id="clickclose_s" style="cursor:pointer;"></span>
        <div class="head1">&nbsp;</div><div class="head2">&nbsp;</div><div class="head3">&nbsp;</div>
        </div>
    <div id="forminner">
    <span id="messagebox"></span>

<form method='post' id='frmemp_resign' name='frmemp_resign' action=""> 
<table><tr>
<td><input type='hidden' id='emp_id' name='emp_id' value='<% response.write(session("emp_id")) %>' />
<input type='hidden' id='emptid' name='emptid' value='<% response.write(session("emptid")) %>' />
Reason<sup style='color:red;'>*</sup></td><td>:</td><td>
<input type='text' id='reason' name='reason' value='' /><br /><label class='lblsmall'></label></td>
<td>Resign Date<sup style='color:red;'>*</sup></td><td>:</td>
<td><input type='text' id='resign_date' name='resign_date' value='' /><br /><label class='lblsmall'></label>
<script language='javascript' type='text/javascript'>
 $(function() {$( "#resign_date").datepicker({changeMonth: true,changeYear: true	}); $( "#resign_date" ).datepicker( "option","dateFormat","mm/dd/yy");});</script>
<script language='javascript' type='text/javascript'> $(function() {$( "#approved_date").datepicker({changeMonth: true,changeYear: true	}); $( "#approved_date" ).datepicker( "option","dateFormat","mm/dd/yy");});</script>
<input type='hidden' id='who_reg' name='who_reg' value="<%response.write(session("username")) %>" />
<input type='hidden' id='date_reg' name='date_reg'  value="<% dim lucur(3) as string
 lucur(2) = Today.Year.ToString
     lucur(1) = Today.Month.ToString
     lucur(0) = Today.Day.ToString
 dim sdate as string=lucur(1) & "/" & lucur(0) & "/" & lucur(2)
 response.write(sdate) %>"/>
</td></tr><tr><td colspan='4'>
<input type='button' name='btnSave' id='btnSave' value='Save' />
<input type='reset' onclick="javascript:$('#btnSave').attr('title','Save');$('#btnSave').attr('value','Save');" /></td></tr></table></form>
    </div>
    <sup style="color:Red;">*</sup>Required Fields
 </div>


    
 <div id="listx" style=" width:100%; padding:0px 7px 0px 0px;">
    <%  Dim db As New dbclass
        Dim dt As DataTableReader
        If keyp = "update" Then
          
      dt = db.dtmake("new" & Today.ToLocalTime, "select * from emp_resign where id=" & Request.QueryString("id"), session("con"))
      If dt.HasRows = True Then
          dt.Read()
          Response.Write("<script type='text/javascript'>")
          For k As Integer = 0 To dt.FieldCount - 4
                    'Response.Write("//" & dt.GetDataTypeName(k).ToLower)
                    If LCase(dt.GetDataTypeName(k)) = "string" And dt.IsDBNull(k) = False Then
                        %>
                    $('#<% Response.Write(dt.GetName(k) & "').val('" & dt.Item(k).trim & "');")%>
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
                    $('#<% Response.Write(dt.GetName(k) & "').val('" & dt.Item(k) & "');")%>
                    <%
                    End If
                    If dt.GetName(k) = "holiday" Then
                        Dim hd() As String
                        If dt.IsDBNull(k) = False Then
                            hd = dt.Item(k).split(",")
                            If hd.Length > 0 Then
                                For i As Integer = 0 To hd.Length - 1
                                    If hd(i) <> "" Then
                                        Response.Write("$('#" & LCase(hd(i)) & "').attr('checked',true);")
                                    End If
                                Next
                            End If
                        End If
                    End If
                Next
                Response.Write("$('#btnSave').attr('title','update');$('#btnSave').attr('value','Update'); $('#formouterbox_small').css('display','block');</script>")
                End If
                dt.Close()
            End If
            db = Nothing
            dt = Nothing
        Dim mk As New formMaker
            Dim row As String
            Dim loc As String
            loc = Request.ServerVariables("url").Substring(1, Request.ServerVariables("url").Length - 1)
        Dim sqlx As String = "select id,emptid,reason,resign_date from emp_resign where emptid='" & Session("emptid") & "' order by id desc"
        row = mk.edit_del_list2("emp_resign", sqlx, "Employment No.,Reason,Date of Resign", session("con"), loc, "", False,true, False, False)
            Response.Write(row)
        
    %>
 </div>
<div id="newpage"></div>
<script type="text/javascript" language="javascript">
hform=findh(document.getElementById("middle_bar"));
    $('.titlet').text("Resign");
    //showobjar("formx","titlet",22,2);
   
  </script>
  
    <script type="text/javascript">
      function del(val,ans,hd)
       {
        
            if(ans=="yes")
            {
           // alert(val + ans);
         $('#frmx').attr("target","_self");
        $('#frmx').attr("action","<% response.write(loc) %>?task=delete&id="+val+"&tbl=emp_resign");
        $('#frmx').submit();
            }
            else
            {
                 $('#frmx').attr("target","_self");
        $('#frmx').attr("action","<% response.write(loc) %>?task=&id="+val+"&tbl=emp_resign");
        $('#frmx').submit();

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
           "<img src='images/gif/btn_delete.gif' style='cursor:pointer;' onclick=" & Chr(34) & "javascript:del('" & idx & "','yes','del123');" & Chr(34) & "></span>"
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
          
           $( "#dialog:ui-dialog" ).dialog( "destroy" );
	
		$( "#dialog-modal" ).dialog({
		resizable: true
			
		});
		
		
	
		           </script>
           <%
           
       End If
           If flg = 1 Then%>
    <script type="text/javascript">
        //$(document).delay(80000);
        $('#frmx').attr("target","_parent");
       
        //$('#frmx').attr("action","<% response.write(rd) %>");
       // $('#frmx').submit();
    </script>
   <%  End If
   
       If Request.QueryString("task") = "addnew" Then
           If addbutton = True Then
           %>
           
               <script type="text/javascript">
        $('#outermessage').text("Add New Data");
      $('#formouterbox_small').css("display","block");
               </script>
        <%  Else
                %>
              <script type="text/javascript">
        $('#outermssage').text("<% response.write(msg) %>");
      $('#outermssage').css("display","block");
              </script><%
           End If
   %>

    <script type="text/javascript">
        $('#messagebox').text("<% response.write(msg) %>");
        $('#messageboxouter').text("<% response.write(msg) %>");
    </script>

    <%
    End If
    
       %>
   
    </body>
</html>
  <script src="scripts/kirsoft.required.js" type="text/javascript"></script>
