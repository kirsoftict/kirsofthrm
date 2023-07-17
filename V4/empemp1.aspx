<%@ Page Language="VB" AutoEventWireup="false" CodeFile="empemp1.aspx.vb" Inherits="empemp1" %>

<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import Namespace="system.net" %>
<%@ Import Namespace="system.IO" %>
<%@ Import namespace="System.Data.sqlclient"%>
<%  Dim client As New WebClient()

Dim fm As New formMaker
    
    'Response.Write(Session("emptid"))
    If Session("emptid").ToString <> "" And Session("emptid").ToString <> "None" Then
        Session("emp_id") = fm.getinfo2("select emp_id from emprec where id='" & Session("emptid") & "'", Session("con"))
    End If
    'Response.Write(Session("emp_id").ToString & "....<br>")
    Dim fl As New file_list
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
      ' Response.Write(sc.d_encryption("zewde@123"))
      Dim rd As String = ""
   
      Dim tbl As String = ""
    Dim key As String = ""
    'Response.Write(Session("emptid"))
    
    Dim keyval As String = ""
    tbl = Request.QueryString("tbl")
    key = Request.QueryString("key")
    rd = Request.QueryString("rd")
    If Request.QueryString.HasKeys = True Then
        If Request.QueryString("dox") = "" Then
            keyval = Request.QueryString("keyval")
            If Request.QueryString("task") = "update" Then
                ' Response.Write("<script type='text/javascript'>alert('updating....');</script>")
                sql = dbx.makeupdate_statement(tbl, Request.QueryString, Session("con"), key, keyval) & Chr(13)
               
                If LCase(Request.QueryString("type_recuritment")) = "contract" Then
                    If fm.getinfo2("select emptid from emp_pen_start where emptid=" & Session("emptid"), Session("con")) = "None" Then
                        ' sql &= ("insert into emp_pen_start(emptid,penstart,date_reg,who_reg) values(" & Session("emptid") & ",'" & Request.QueryString("hire_date") & "','" & Today.ToShortDateString & "','system')") & Chr(13)
                    End If
                    
                End If
                sql = "Begin Trans " & Session("emp_iid") & Chr(13) & sql
                Dim flg3 As String
                flg3 = dbx.excutes(sql, Session("con"),session("path"))
                If IsNumeric(flg3) Then
                    If CInt(flg3) > 0 Then
                        flg3 = dbx.excutes("Commit Trans " & Session("emp_iid"), Session("con"), Session("path"))
                        msg = "data is updated"
                        Response.Write(msg)
                        If CInt(flg3) <> -1 Then
                            dbx.excutes("RollBack Trans " & Session("emp_iid"), Session("con"),session("path"))
                            msg = "sorry data is not updated"
                        Else
                            client.DownloadFile(Session("http_host") & "/" & "reportnonactive.aspx?val=all", Session("path") & "\reportnon.html")
                            client.DownloadFile(Session("http_host") & "/" & "viewreport1.aspx?val=all&active=y", Session("path") & "\reportactive.html")
                        End If
                    End If
                End If
               
            ElseIf Request.QueryString("task") = "delete" Then
                'Response.Write("<script type='text/javascript'>alert('deleting....');</script>")
                ' sql = dbx.makeupdate_statement(tbl, Request.QueryString, session("con"), key, keyval)
                sql = "delete from " & Request.QueryString("tbl") & " where id=" & Request.QueryString("id") '
                flg = dbx.excutes(sql, Session("con"), Session("path"))
                   
                ' Response.Write(sql)
                If flg = 1 Then
                    msg = "Data deleted"
                    client.DownloadFile(Session("http_host") & "/" & "reportnonactive.aspx?val=all", Session("path") & "\reportnon.html")
                    client.DownloadFile(Session("http_host") & "/" & "viewreport1.aspx?val=all&active=y", Session("path") & "\reportactive.html")
                End If
            ElseIf Request.QueryString("task") = "save" Then
                ' Response.Write("<script type='text/javascript'>alert('saving....');</script>")

                sql = dbx.makest(tbl, Request.QueryString, Session("con"), key) & Chr(13)
                ' Response.Write(sql)
                If LCase(Request.QueryString("type_recuritment")) = "contract" Then
                    If fm.getinfo2("select emptid from emp_pen_start where emptid=" & Session("emptid"), Session("con")) = "None" Then
                        '  sql &= ("insert into emp_pen_start(emptid,penstart,date_reg,who_reg) values(" & Session("emptid") & ",'" & Request.QueryString("hire_date") & "','" & Today.ToShortDateString & "','system')") & Chr(13)
                    End If
                    
                End If
                sql = "Begin Tran hiret " & Chr(13) & sql
                Dim flg3 As String
                flg3 = dbx.excutes(sql, Session("con"),session("path"))
                If IsNumeric(flg3) Then
                    If CInt(flg3) > 0 Then
                        Session("emptid") = fm.getinfo2("select Max(id) from emprec where emp_id='" & Session("emp_id") & "'", Session("con"))
                        flg3 = dbx.excutes("COMMIT Tran hiret", Session("con"), Session("path"))
                        msg = "data is Saved"
                        
                        client.DownloadFile(Session("http_host") & "/" & "reportnonactive.aspx?val=all", Session("path") & "\reportnon.html")
                        client.DownloadFile(Session("http_host") & "/" & "viewreport1.aspx?val=all&active=y", Session("path") & "\reportactive.html")
                        If CInt(flg3) <> -1 Then
                            dbx.excutes("ROLLBACK trans hiret", Session("con"), Session("path"))
                            msg = "sorry data is not Saved"
                        End If
                    End If
                End If
            Else
                flg = 1
                msg = ""
                
               
            End If
            'MsgBox(rd)
         
            ' sql = db.makest(tbl, Request.QueryString, session("con"), key)
           
            If flg <> 1 Then
                ' Response.Write(Request.Form("type_recuritment"))
                ' Response.Write("<span style='font-size:15pt; color:Red;'>Sorry Data doesnt change </span>")
            End If
          
   
        End If
    End If

   %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title></title>

<script language="javascript" type="text/javascript" src="jqq/jquery-1.9.1.js"></script>
<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />

<link rel="stylesheet" type="text/css" media="screen" href="css/kir.login.css" /> 
<script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script src="scripts/script.js" type="text/javascript"></script>
<script type="text/javascript" src="jqq/ui/jquery.ui.core.js"></script>
<script type="text/javascript" src="jqq/ui/jquery.ui.widget.js"></script>
	<script type="text/javascript"  src="jqq/ui/jquery.ui.mouse.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.draggable.js"></script>
	<script type="text/javascript" src="scripts/form.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.position.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.resizable.js"></script>
	<script type="text/javascript" src="jqq/ui/jquery.ui.datepicker.js"></script>
	<!--script type="text/javascript" src="jq/ui/jquery.ui.button.js"></script-->
     <script src="jqq/ui/jquery.ui.button.js"></script>

    <script src="jqq/ui/jquery.ui.dialog.js"></script>
	
	<link rel="stylesheet" href="jq/demos.css">

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
var requf=["emp_id","job_position","department","type_recuritment","hire_date","x"];
var fieldlist=["emp_id","job_position","department","type_recuritment","project_id","hire_date","who_reg","date_reg","active","x"];
function validation1(){if ($('#emp_id').val() == '') {showMessage('emp_id cannot be empty','emp_id');$('#emp_id').focus();return false;}
if ($('#job_position').val() == '') {showMessage('job_position cannot be empty','job_position');$('#job_position').focus();return false;}
if ($('#department').val() == '') {showMessage('department cannot be empty','department');$('#department').focus();return false;}
if ($('#type_recuritment').val() == '') {showMessage('type_recuritment cannot be empty','type_recuritment');$('#type_recuritment').focus();return false;}
if ($('#hire_date').val() == '') {showMessage('hire_date cannot be empty','hire_date');$('#hire_date').focus();return false;}
else if(focused=="") { var ans;
ans=checkblur();
if(ans!=true){ 
 $("#" + ans).focus();
}else{
   var str=$("#frmemprec").formSerialize();
   $("#frmemprec").attr("action","?tbl=emprec&task=<% response.write(keyp) %>&lrd=empcontener&key=id&keyval=<% response.write(idx) %>&rd=empcontener.aspx&" + str);
    $("#frmemprec").submit();
  return true;}
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
function changeactive()
{
    if($("#end_date").val()=="")
    {
    $("#active").val("y");
    }
    else
    {
    $("#active").val("n");
    }
}
</script>

</head>

<body style="height:auto;">

<div>
 </div>
    <% If Session("username") = "" Then
       %>
       <script type="text/javascript">
 //document.location.href="admin_home.php"
window.location="logout.aspx";
</script>
       <%
       End If
       
       If Session("emp_id").ToString = "" Then
       %>

<script type="text/javascript">
 //document.location.href="admin_home.php"
window.location="empcontener.aspx";
</script>

<%
Else
     %>

<script type="text/javascript">
//alert('<% 'response.write(session("emp_id")) %>');
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
Try
    Response.Write(Session("emp_id").ToString)

    sql = "select * from emprec where emp_id='" & Session("emp_id") & "' order by id desc"
    Dim dtt As DataTableReader
    dtt = dbx.dtmake("onerect", sql, Session("con"))
    If dtt.HasRows = True Then
        dtt.Read()
        If dtt.IsDBNull(8) = True Then
            addbutton = False
            msg = "Add Button is Disabled this person contract not yet end or Not Update his data"
        Else
            If dtt.Item("end_date").ToString = "" Then
                addbutton = False
                msg = "Add Button is Disabled this person contract not yet end or NOt Update his data"
            End If
        End If
    Else
        msg = "Add New Data"
    End If
    dtt.Close()
    Response.Write(fl.dialog("pay", "", "<iframe name='fpay' id='fpay' width='500' height='500' frameborder='0' src='' scrolling='no'></iframe>"))
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
   <form method='post' id='frmemprec' action="" name='frmemprec'> 
<table><tr><td>
<input type='hidden' id='emp_id' name='emp_id' value="<%response.write(session("emp_id")) %>" />
Employeement Type<sup style='color:red;'>*</sup></td><td>:</td><td>
<select id='type_recuritment' name='type_recuritment'>
<option value="">--select--</option>
<%  
    Response.Write(fm.getoption("tbl_emp_type where status='y' order by emptype", "emptype", "emptype", session("con")))%>

<%  
    'Response.Write(fm.getoption("tbldepartment", "dep_id", "dep_name", session("con")))%>

</select>
<br /><label class='lblsmall'></label></td>
</tr>
<tr><td colspan="6"><hr /></td></tr>
<tr><td colspan="6"> <input type="hidden" id="holiday" name="holiday" value="Sunday" /><table><tr><th colspan="4"><span>Weekend (Holiday)</span><sup style='color:red;'>*</sup></th></tr>
<tr><td><input type='checkbox' id='sunday'  value="Sunday" <%  If keyp <> "update" Then %>checked="checked"<%end if %> onclick="javascript: checkchk('sunday');" />Sunday
</td><td><input type='checkbox' id='monday' value="Monday" onclick="javascript: checkchk('monday');" />Monday
</td><td><input type='checkbox' id='tuesday' value="Tuesday" onclick="javascript: checkchk('tuesday');"/>Tuesday
</td></tr>
<tr><td><input type='checkbox' id='wednesday'  value="Wednesday" onclick="javascript: checkchk('wednesday');"/>Wednesday
</td><td><input type='checkbox' id='thrusday' value="Thrusday" onclick="javascript: checkchk('thrusday');"/>Thrusday
</td><td><input type='checkbox' id='friday' value="Friday" onclick="javascript: checkchk('friday');" />Friday
</td><td><input type='checkbox' id='saturday' value="Saturday"onclick="javascript: checkchk('saturday');" />Saturday
</td></tr></table>
</td>
</tr><tr><td colspan="6"><hr /></td></tr>
<tr>
<td>Employment date<sup style='color:red;'>*</sup></td><td>:</td><td>
<input type='text' id='hire_date' name='hire_date' /><br /><label class='lblsmall'></label></td>
<td>End Date<sup style='color:red;'>*</sup></td><td>:</td><td>
<input type='text' id='end_date' name='end_date' onchange="changeactive()" /><br /><label class='lblsmall'></label></td>
</tr>
<tr><td>
<input type='hidden' id='who_reg' name='who_reg' value="<%response.write(session("username")) %>" />
<input type='hidden' id='date_reg' name='date_reg'  value="<% dim lucur(3) as string
 lucur(2) = Today.Year.ToString
     lucur(1) = Today.Month.ToString
     lucur(0) = Today.Day.ToString
 dim sdate as string=lucur(1) & "/" & lucur(0) & "/" & lucur(2)
 response.write(sdate) %>"/>
 <br /><label class='lblsmall'></label>
active<sup style='color:red;'>*</sup></td><td>:</td><td>
<input type='text' id='active' name='active' value="y" /><br /><label class='lblsmall'></label></td>
</tr><tr><td colspan='4'><input type='button' name='btnSave' id='btnSave' value='Save' />

<input type='reset' onclick="javascript:$('#btnSave').attr('title','Save');$('#btnSave').attr('value','Save');" /></td></tr></table></form>
    </div>
    <sup style="color:Red;">*</sup>Required Fields
 </div>


    
 <div id="listx" style=" width:100%; padding:0px 7px 0px 0px;">
    <%  dbx = Nothing
        Dim db As New dbclass
        Dim dt As DataTableReader
        If keyp = "update" Then
          
      dt = db.dtmake("new" & Today.ToLocalTime, "select * from emprec where id=" & Request.QueryString("id"), session("con"))
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
        '  dt=db.dtmake("emrr","select id from emprec where id='" & Session("emptid") & "' and type_ 
            db = Nothing
            dt = Nothing
        Dim mk As New formMaker
            Dim row As String
            Dim loc As String
            loc = Request.ServerVariables("url").Substring(1, Request.ServerVariables("url").Length - 1)
        Dim sqlx As String = "select id,type_recuritment,hire_date,end_date from emprec where emp_id='" & Session("emp_id") & "' order by hire_date desc"
        row = mk.edit_del_list("emprec", sqlx, "Emp. Type,Employment date,End Date", session("con"), loc)
            Response.Write(row)
        
    %>
 </div>
<div id="newpage"></div>
<script type="text/javascript" language="javascript">
hform=findh(document.getElementById("middle_bar"));
    $('.titlet').text("Employeement Detail");
    //showobjar("formx","titlet",22,2);
   
  </script>
  
    <script type="text/javascript">
      function del(val,ans,hd)
       {
        
            if(ans=="yes")
            {
           // alert(val + ans);
         $('#frmx').attr("target","_self");
        $('#frmx').attr("action","<% response.write(loc) %>?task=delete&id="+val+"&tbl=emprec");
        $('#frmx').submit();
            }
            else
            {
                 $('#frmx').attr("target","_self");
        $('#frmx').attr("action","<% response.write(loc) %>?task=&id="+val+"&tbl=emprec");
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
           Dim fex As String
           fex = fm.getinfo2("select count(emptid) from payrollx where emptid=" & idx, Session("con"))
           
           con = "<span style='color:red;'> This row of data will not be come again. "
           
           If fex <> "None" And CInt(fex) > 0 Then
               con &= " And also its affect the payroll " & fex & " rows"
           End If
           con &= "<br />Are you sure you want delete it?" & idx & "<br /><hr>" & _
           "<img src='images/gif/btn_delete.gif' style='cursor:pointer;' onclick=" & Chr(34)
           If fex = "None" Then
               
               con &= "javascript:del('" & idx & "','yes','del123');"
           Else
               If IsNumeric(fex) Then
                   If CInt(fex) > 0 Then
                       con &= Chr(34)
                   Else
                       con &= "javascript:del('" & idx & "','yes','del123');" & Chr(34)
                   End If
               End If
               
           End If
               
           con &= "></span>"
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
    fm = Nothing
    If LCase(Request.QueryString("type_recuritment")) = "contract" Then
        If flg = 1 Then
            Response.Write("<script>")
            
            Response.Write("  $('#frmx').attr('target', 'fpay');")
            Response.Write("$('#fpay').attr('frameborder', '0');")
            Response.Write(" $('#frmx').attr('action', 'contract_entry.aspx?task=contractnew&conday=" & Request.QueryString("hire_date") & "');")
            Response.Write("$('#frmx').submit();")
         
            Response.Write("$('#pay').css({ top: '0px', left: '0px' });")
            Response.Write("$('#pay').remove('display');")
            Response.Write("$('#pay').dialog({")
            Response.Write(" height: 300,")
            Response.Write("width: 600,")
            Response.Write(" modal: true")
            Response.Write("  }); ")
     Response.Write("</script>")
        End If
                       
                        
                        
    End If
    outp()
Catch ex As Exception
    Response.Write(ex.ToString & "<br>")

End Try
       %>
   
    </body>
</html>
  <script src="scripts/kirsoft.required.js" type="text/javascript"></script>
