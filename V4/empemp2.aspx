<%@ Page Language="VB" AutoEventWireup="false" CodeFile="empemp2.aspx.vb" Inherits="empemp2" %>

<%@ Import Namespace="kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<% 
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
        Dim keyval As String = ""
        tbl = Request.QueryString("tbl")
        key = Request.QueryString("key")
        rd = Request.QueryString("rd")
        If Request.QueryString.HasKeys = True Then
            If Request.QueryString("dox") = "" Then
                keyval = Request.QueryString("keyval")
                If Request.QueryString("task") = "update" Then
                ' Response.Write("<script type='text/javascript'>alert('updating....');</script>")
                    sql = dbx.makeupdate_statement(tbl, Request.QueryString, session("con"), key, keyval)
                    flg = dbx.save(sql, session("con"),session("path"))
                    ' Response.Write(sql)
                    If flg = 1 Then
                    msg = "Data Updated"
                    End If
                ElseIf Request.QueryString("task") = "delete" Then
                'Response.Write("<script type='text/javascript'>alert('deleting....');</script>")
                    ' sql = dbx.makeupdate_statement(tbl, Request.QueryString, session("con"), key, keyval)
                    sql = "delete from " & Request.QueryString("tbl") & " where id=" & Request.QueryString("id") '
                    flg = dbx.save(sql, session("con"),session("path"))
                   
                    ' Response.Write(sql)
                    If flg = 1 Then
                    msg = "Data deleted"
                End If
                ElseIf Request.QueryString("task") = "save" Then
                ' Response.Write("<script type='text/javascript'>alert('saving....');</script>")

                    sql = dbx.makest(tbl, Request.QueryString, session("con"), key)
                ' Response.Write(sql)
                flg = dbx.save(sql, Session("con"), Session("path"))
                
                    If flg = 1 Then
                    msg = "Data Saved"
                    For Each t As String In Request.ServerVariables
                        Response.Write(Request.ServerVariables(t) & "<br>")
                    Next
                End If
            Else
                flg = 1
                msg = ""
                
               
            End If
            'MsgBox(rd)
         
            ' sql = db.makest(tbl, Request.QueryString, session("con"), key)
           
            If flg <> 1 Then
                Response.Write("<span style='font-size:15pt; color:Red;'>Sorry Data doesnt change </span>")
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
    <script src="scripts/kirsoft.java.js" type="text/javascript"></script>
<script type="text/javascript">
	$(function() {
		$( "#date_from" ).datepicker({
			defaultDate: "+1w",
			changeMonth: true,
			changeYear:true,
			numberOfMonths: 2,
			minDate: "-70Y", maxDate: "-1d",
			onSelect: function( selectedDate ) {
				$( "#date_end" ).datepicker( "option", "minDate", selectedDate );
				
			}
		});
		$( "#date_end" ).datepicker( "option","dateFormat","mm/dd/yy");
		$( "#date_from" ).datepicker( "option","dateFormat","mm/dd/yy");
		$( "#date_end" ).datepicker({
			defaultDate: "+1W",
			changeMonth: true,
			changeYear:true,
			numberOfMonths: 2,
			maxDate: "+1Y",
			onSelect: function( selectedDate ) {
				$( "#date_from" ).datepicker( "option", "MaxDate", selectedDate );
			}
		});
	});
	</script>
<script type="text/javascript">
var prv;
  prv="";
var id;
var focused="";
var requf=["emp_id","position","department","ass_for","x"];
var fieldlist=["emp_id","position","department","type_recuritment","project_id","date_from","who_reg","date_reg","active","x"];
function validation1() {


    if ($('#position').val() == '') { showMessage('job_position cannot be empty', 'position'); $('#position').focus(); return false; }
if ($('#department').val() == '') {showMessage('department cannot be empty','department');$('#department').focus();return false;}
if ($('#date_from').val() == '') {showMessage('date_from cannot be empty','date_from');$('#date_from').focus();return false;}
if ($('#ass_for').val() == '') {showMessage('Assignment for cannot be empty','ass_for');$('#ass_for').focus();return false;}

else if(focused=="") { var ans;
ans=checkblur();
if(ans!=true){ 
 $("#" + ans).focus();
}else{
   var str=$("#frmemp_job_assign").formSerialize();
   $("#frmemp_job_assign").attr("action","?tbl=emp_job_assign&task=<% response.write(keyp) %>&lrd=empcontener&key=id&keyval=<% response.write(idx) %>&rd=empcontener.aspx&" + str);
    $("#frmemp_job_assign").submit();
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
</script>

</head>

<body style="height:auto;">

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

'Response.Write("<br />" & Request.Form("do"))
Dim fm As New formMaker
sql = "select * from emp_job_assign where emp_id='" & Session("emp_id") & "' order by id desc"
Dim dtt As DataTableReader
dtt = dbx.dtmake("onerect", sql, Session("con"))
Dim idon As String = fm.getinfo2("select hire_date from emprec where end_date is NULL and emp_id='" & Session("emp_id") & "'", Session("con"))

If dtt.HasRows = True Then
    dtt.Read()
    If dtt.IsDBNull(8) = True Then
        addbutton = False
        msg = "Add Button is Disabled this person contract not yet end or Not Update his data"
    Else
        If dtt.Item("date_end").ToString = "" Then
            addbutton = False
            msg = "Add Button is Disabled this person contract not yet end or NOt Update his data"
        End If
    End If
Else
    msg = "Add New Data"
End If

If idon = "None" Then
    addbutton = False
    msg = "Add Button is Disabled this person Hired information not incurred"
    idon = ""
Else
    idon = CDate(idon).ToShortDateString
End If

dtt.Close()
Dim namelist As String

namelist = fm.getjavalist2("emp_static_info", "first_name,middle_name,last_name", Session("con"), " ")
'Response.Write(Session("emptid"))
'Response.Write(Session("emp_id"))
 %><script>var namelist=[<% response.write(namelist) %>];</script>

 <div style="top:0px;height:45px; width:80px; background:url(images/gif/add_new.gif) no-repeat; cursor:pointer" id="addbutton" onclick="javascript: $('#frmx').attr('target','_self'); $('#frmx').attr('action','<% response.write(Request.ServerVariables("URL")) %>?task=addnew'); $('#frmx').submit();">
     </div>
     <div id="outermssage" style="width:75%; height:25px; display:none; color:Green; background:url(images/gif/warn16_1.gif) no-repeat; text-align:center; font-weight:bold; font-size:12pt;"></div>
 <div id="formouterbox_small" style="display:none; width:800px;">
     <div id="formheader">
    <span class="titlet">
This is Title</span>
<span class="close" id="clickclose_s" style="cursor:pointer;"></span>
        <div class="head1">&nbsp;</div><div class="head2">&nbsp;</div><div class="head3">&nbsp;</div>
        </div>
    <div id="forminner">
    <span id="messagebox"></span>
   <form method='post' id='frmemp_job_assign' action="" name='frmemp_job_assign'> 
<table style='width:400px;' width=800>
<tr><td>
<input type='hidden' id='emp_id' name='emp_id' value='<% response.write(session("emp_id")) %>' />
<input type='hidden' id='emptid' name='emptid' value='<% response.write(session("emptid")) %>' />
Job Position<sup style='color:red;'>*</sup></td>
<td>
<select id='position' name='position' style='max-width:400px; overflow:auto;'>
<option value="">--Select Position--</option>
<%  
    Response.Write(fm.getoption("tblposition order by job_position ", "job_position", "job_position", session("con")))%>
</select>
<br /><label class='lblsmall'></label></td>
</tr>
<tr><td>Department<sup style='color:red;'>*</sup></td><td>
<select id='department' name='department' >
<option value="">--Select Department--</option>
<%  
    Response.Write(fm.getoption("tbldepartment", "dep_id", "dep_name", session("con")))%>
</select>
<br /><label class='lblsmall'></label></td>
</tr><tr>
<td>IS Department Head<sup style='color:red;'></sup></td><td>
<select id='ishead' name='ishead' >
<option value="">--Select yes/no--</option>
<option value="y">Yes</option>
<option value="n">No</option>
</select>
</td></tr><tr>
<td>Submited to<sup style='color:red;'></sup></td><td>
<input type="text" id='submited_to' name='submited_to' onkeyup="javascript: multiselect('submited_to',namelist);" />

<br /><label class='lblsmall'></label></td></tr>
<tr>
<td>Assignment for<sup style='color:red;'>*</sup></td><td>
<select id='ass_for' name='ass_for'>
<option value="">--select--</option>
<option value="Hired">Hired</option>
<option value="Promotion">Promotion</option>
<option value="Demotion">Demotion</option>
<option value="Linear">Linear</option>
<option value="Other">Other</option>
<option value="Job Assignment">Job Assignment</option>

</select>
<br /><label class='lblsmall'></label></td>
</tr><tr>
<td>Project<sup style='color:red;'></sup></td><td>
<select id='project_id' name='project_id' style='max-width:400px; overflow:auto;'>
<option value="">--Project--</option>
<%  'Dim dt As DataTableReader
    '  dt = dbx.dtmake("static" & Today.ToLongDateString, "select * from emp_static_info", session("con"))
   
    Response.Write(fm.getoption("tblproject", "project_id", "project_name", Session("con")))%>
</select>
<br /><label class='lblsmall'></label></td></tr>
<tr><td colspan="6"><hr /></td></tr>
<tr>
<td>Date From<sup style='color:red;'>*</sup>
<input type='text' id='date_from' name='date_from' value="<% Response.write(idon) %>"/><br /><label class='lblsmall'></label></td>
<td>End Date<sup style='color:red;'>*</sup>
<input type='text' id='date_end' name='date_end' /><br /><label class='lblsmall'></label></td>
</tr>
<tr><td>
<input type='hidden' id='who_reg' name='who_reg' value="<%response.write(session("username")) %>" />
<input type='hidden' id='date_reg' name='date_reg'  value="<% dim lucur(3) as string
 lucur(2) = Today.Year.ToString
     lucur(1) = Today.Month.ToString
     lucur(0) = Today.Day.ToString
 dim sdate as string=lucur(1) & "/" & lucur(0) & "/" & lucur(2)
 response.write(sdate) %>"/>

<input type='hidden' id='active' name='active' value="y"  /></td>
</tr><tr><td colspan='4'><input type='button' name='btnSave' id='btnSave' value='Save' />

<input type='reset' onclick="javascript:$('#btnSave').attr('title','Save');$('#btnSave').attr('value','Save');" /></td></tr></table></form>
    </div>
    <sup style="color:Red;">*</sup>Required Fields
 </div>


    
 <div id="listx" style=" width:100%; padding:0px 7px 0px 0px;">
    <%  Dim db As New dbclass
        Dim dt As DataTableReader
        If keyp = "update" Then
          
      dt = db.dtmake("new" & Today.ToLocalTime, "select * from emp_job_assign where id=" & Request.QueryString("id"), session("con"))
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
           
        Dim mk As New formMaker
        Dim row As String
        Dim loc As String
        loc = Request.ServerVariables("url").Substring(1, Request.ServerVariables("url").Length - 1)
        Dim sqlx As String = "select id,position,department,project_id,ishead,date_from,date_end from emp_job_assign where emp_id='" & Session("emp_id") & "' order by id desc"
        row = mk.edit_del_list("emp_job_assign", sqlx, "Position,Dempartment,Project,submited_to,Assignment Date,End Date", Session("con"), loc)
        Response.Write(row)
        Dim csal() As String
        If Session("emp_id") = "" Then
       %>
       <script type="text/javascript">
 //document.location.href="admin_home.php"
window.location="emplist.aspx";
</script>
       <%
       End If
       Try
           If IsNumeric(CInt(Session("emptid"))) Then
               
           End If
       Catch ex As Exception
           Session("emptid") = "xx"
       End Try
       If IsNumeric((Session("emptid"))) Then
           csal = dbx.getsal(CInt(Session("emptid")), Session("con"))
           ' Response.Write(Session("emptid").ToString)
           'FormatNumber(csal(0), 2, TriState.True, TriState.True, TriState.True)
           ' Response.Write(csal)
           If IsNumeric(csal(0)) Then
               Response.Write("<br><br><div style='font-weight:bold;'>Current Salary:&nbsp;" & FormatNumber(csal(0).ToString, 2, TriState.True, TriState.True, TriState.True).ToString & "</div>")
               'Response.Write(csal(0).ToString)
           End If
           
       End If
       
       If row.Length <= 731 Then
           dt = db.dtmake("getenfo", "select * from emprec where emp_id='" & Session("emp_id") & "' and end_date is Null", Session("con"))
           If dt.HasRows Then
               dt.Read()
               Response.Write("<script type='text/javascript'>$('#date_from').val('" & dt.Item("hire_date") & "');</script>")
                    
           End If
           dt.Close()
       End If
       db = Nothing
       dt = Nothing
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
        $('#frmx').attr("action","<% response.write(loc) %>?task=delete&id="+val+"&tbl=emp_job_assign");
        $('#frmx').submit();
            }
            else
            {
                 $('#frmx').attr("target","_self");
        $('#frmx').attr("action","<% response.write(loc) %>?task=&id="+val+"&tbl=emp_job_assign");
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
    <script src="scripts/kirsoft.required.js" type="text/javascript"></script>
    </body>
</html>
 
