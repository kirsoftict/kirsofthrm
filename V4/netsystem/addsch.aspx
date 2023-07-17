<%@ Page Language="VB" AutoEventWireup="false" CodeFile="addsch.aspx.vb" Inherits="netsystem_addsch" %>

<%@ Import Namespace="Kirsoft.hrm" %>
<%@ Import Namespace="system.data" %>
<%@ Import namespace="System.Data.sqlclient"%>
<!doctype html>
<html lang="en">

<head>
    <meta charset="utf-8">
    <meta http-equiv="x-ua-compatible" content="ie=edge">
    <title>Net Consult:System</title>
    <meta name="description" content="">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- favicon
		============================================ -->
    <link rel="shortcut icon" type="image/x-icon" href="<%=session("logo") %>">
    <!-- Google Fonts
		============================================ -->
    <link href="https://fonts.googleapis.com/css?family=Roboto:100,300,400,700,900" rel="stylesheet">
    <!-- Bootstrap CSS
		============================================ -->
    <link rel="stylesheet" href="css/bootstrap.min.css">
    <!-- Bootstrap CSS
		============================================ -->
   
    <link href="css/bootstrap-datetimepicker.css" rel="stylesheet">
    <!-- owl.carousel CSS
		============================================ -->
    <link rel="stylesheet" href="css/owl.carousel.css">
    <link rel="stylesheet" href="css/owl.theme.css">
    <link rel="stylesheet" href="css/owl.transitions.css">
    <link rel="Stylesheet" href="css/base.css"
    <!-- animate CSS
		============================================ -->
    <link rel="stylesheet" href="css/animate.css">
    <!-- normalize CSS
		============================================ -->
    <link rel="stylesheet" href="css/normalize.css">
    <!-- meanmenu icon CSS
		============================================ -->
    <link rel="stylesheet" href="css/meanmenu.min.css">
    <!-- main CSS
		============================================ -->
    <link rel="stylesheet" href="css/main.css">
    <!-- educate icon CSS
		============================================ -->
    <link rel="stylesheet" href="css/educate-custon-icon.css">
    <!-- morrisjs CSS
		============================================ -->
    <link rel="stylesheet" href="css/morrisjs/morris.css">
    <!-- mCustomScrollbar CSS
		============================================ -->
    <link rel="stylesheet" href="css/scrollbar/jquery.mCustomScrollbar.min.css">
    <!-- metisMenu CSS
		============================================ -->
    <link rel="stylesheet" href="css/metisMenu/metisMenu.min.css">
    <link rel="stylesheet" href="css/metisMenu/metisMenu-vertical.css">
    <!-- calendar CSS
		============================================ -->
    <link rel="stylesheet" href="css/calendar/fullcalendar.min.css">
    <link rel="stylesheet" href="css/calendar/fullcalendar.print.min.css">
    <!-- Preloader CSS
		============================================ -->
    <link rel="stylesheet" href="css/preloader/preloader-style.css">
    <!-- style CSS
    <link rel="stylesheet" href="css/calendar/fullcalendar.min.css">
    <link rel="stylesheet" href="css/calendar/fullcalendar.print.min.css">
    <!-- summernote CSS
		============================================ -->
    <link rel="stylesheet" href="css/summernote/summernote.css">

		

		============================================ -->
    <link rel="stylesheet" href="style.css">
    <!-- responsive CSS
		============================================ -->
    <link rel="stylesheet" href="css/responsive.css">
    <!-- modernizr JS
		============================================ -->
    <script rc="js/vendor/modernizr-2.8.3.min.js"></script>
     <!-- jquery
		============================================ -->
    <script src="js/vendor/jquery-1.12.4.min.js"></script>
    <!-- bootstrap JS
		============================================ -->
    <script src="js/bootstrap.min.js"></script>
   <!-- <script type="text/javascript" src="./eonasdan.github.io_files/bootstrap.min.js.download"></script>-->
        
         
         <script src="js/moment-with-locales.js"></script>
        <script src="js/bootstrap-datetimepicker.js"></script>
    <!-- wow JS
		============================================ -->
    <script src="js/wow.min.js"></script>
    <!-- price-slider JS
		============================================ -->
    <script src="js/jquery-price-slider.js"></script>
    <!-- meanmenu JS
		============================================ -->
    <script src="js/jquery.meanmenu.js"></script>
    <!-- owl.carousel JS
		============================================ -->
    <script src="js/owl.carousel.min.js"></script>
    <!-- sticky JS
		============================================ -->
    <script src="js/jquery.sticky.js"></script>
    <!-- scrollUp JS
		============================================ -->
    <script src="js/jquery.scrollUp.min.js"></script>
    <!-- mCustomScrollbar JS
		============================================ -->
    <script src="js/scrollbar/jquery.mCustomScrollbar.concat.min.js"></script>
    <script src="js/scrollbar/mCustomScrollbar-active.js"></script>
    <!-- metisMenu JS
		============================================ -->
    <script src="js/metisMenu/metisMenu.min.js"></script>
    <script src="js/metisMenu/metisMenu-active.js"></script>
       <!-- datapicker JS
		============================================ -->
   

    <!-- morrisjs JS
		============================================ -->
    <script src="js/sparkline/jquery.sparkline.min.js"></script>
    <script src="js/sparkline/jquery.charts-sparkline.js"></script>
    <!-- summernote JS
		============================================ -->
    <script src="js/summernote/summernote.min.js"></script>
    <script src="js/summernote/summernote-active.js"></script>

    <!-- calendar JS
		============================================ -->
    <script src="js/calendar/moment.min.js"></script>
    <script src="js/calendar/fullcalendar.min.js"></script>
    <script src="js/calendar/fullcalendar-active.js"></script>
    <!-- tab JS
		============================================ -->
    <script src="js/tab.js"></script>
    <!-- plugins JS
		============================================ -->
    <script src="js/plugins.js"></script>
    <!-- main JS
		============================================ -->
    <script src="js/main.js"></script>
    </head>
<%  If Session("username") = "" Then
       %>
       <script type="text/javascript">
           //document.location.href="admin_home.php"
           // window.location = "logout.aspx";
</script>
       <%
       End If
       
       If Session("username") = "" Then
           Response.Redirect("../login.aspx")
       %>

<script type="text/javascript">
    //document.location.href="admin_home.php"
   // window.location = "empcontener.aspx";

</script>

<%
ElseIf Session("emptid") = "0" Then

     %>

<script type="text/javascript">
    alert('<% response.write(session("emptid")) %>');
</script>

<%    
End If

    dim emptid as string=session("emptid")
    Dim keyp As String = ""
    Dim paths, loc As String
    Dim fl As New file_list
    Dim fm As New formMaker
    If Request.QueryString("dox") = "edit" Then
        keyp = "update"
    ElseIf Request.QueryString("dox") = "delete" Then
        keyp = "delete"
    ElseIf Request.QueryString("dox") = "upload" Then
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
            sql = dbx.makeupdate_statement(tbl, Request.QueryString, session("conx"), key, keyval)
            Response.Write(sql)
            flg = dbx.save(sql, session("conx"),session("path"))
            Dim val As Double
            val = Request.QueryString("no_days")
            Dim rs As DataTableReader
            rs = dbx.dtmake("empleave", "select * from empleavapp where req_id='" & keyval & "' order by id desc", session("conx"))
            If rs.HasRows = True Then
                While rs.Read
                    If CDbl(rs.Item("used")) - CDbl(val) >= 0 Then
                        msg = "update empleavapp set used='" & val.ToString & "' where id=" & rs.Item("id")
                        dbx.save(msg, session("conx"), Session("path"))
                    Else
                        msg = "update empleavapp set used='" & rs.Item("used") & "' where id=" & rs.Item("id")
                        val = val - CDbl(rs.Item("used"))
                        dbx.save(msg, session("conx"), Session("path"))
                    End If
                End While
                
            End If
            rs.Close
                    ' Response.Write(sql)
                    If flg = 1 Then
                msg = "Data Updated"
                    End If
                ElseIf Request.QueryString("task") = "delete" Then
              
                    sql = "delete from " & Request.QueryString("tbl") & " where id=" & Request.QueryString("id") '
                    flg = dbx.save(sql, session("conx"),session("path"))
                   
                    ' Response.Write(sql)
            If flg = 1 Then
                sql = "delete from empleavapp where req_id=" & Request.QueryString("id") 'deleted from budget
                flg = dbx.save(sql, session("conx"),session("path"))
                msg = "Data deleted"
            End If
                ElseIf Request.QueryString("task") = "save" Then
                If Request.QueryString("date_taken_from") <> "" Then
                Dim dtstr() As String = Request.QueryString("date_taken_from").Split("/")
                flg = 0
                Dim dtc As New datetimecal
                Dim ksys As New kirsoftsystem
                    
                Dim ddd As Date
                ddd = "#" & Request.QueryString("date_taken_from") & "#" '"#" & dtstr(1) & "/" & dtstr(0) & "/" & dtstr(2) & "#"
                    
                '  ddd = ksys.StringToDate((Request.QueryString("date_taken_from")), "dd/mm/yyyy")
                Dim obj, obj2 As Object
                obj = dtc.isWeekEnd(ddd, Session("emp_id"), session("conx"))
                obj2 = dtc.isPublic(ddd, session("conx"))
                msg = obj.ToString
                    
                If obj.ToString = "True" Then
                    flg = 2
                    msg = "This day is your weekend, please select another day"
                End If
                If obj2 = True Then
                    flg = 2
                    msg = "This day is Public Holiday"
                End If
                If obj2 = True And obj.ToString = "True" Then
                    flg = 2
                    msg = "This day is Public and week holiday"
                End If
                If flg <> 2 Then
                    sql = dbx.makest(tbl, Request.QueryString, session("conx"), key)
                    ' Response.Write(sql)
                    flg = dbx.save(sql, session("conx"),session("path"))
                    If flg = 1 Then
                        msg = "Data Saved"
                    Else
                        msg = "Sorry Data is not Save"
                    End If
                End If
                    
            Else
                    
                msg = "it is else"
            End If
               
                ' Response.Write(sql)
               
            ElseIf Request.QueryString("task") = "deletefile" Then
                Response.Write("adfadf")
                
                If Request.QueryString("id") <> "" Then
                    
                    paths = Request.QueryString("id")
                    paths = paths.Replace("~", "\")
                    msg = fl.deletefile(paths)
                End If
                flg = 1
            End If
               
            If flg <> 1 Then
                'Response.Write("<span style='font-size:15pt; color:Red;'>Sorry Data doesnt change </span>")
            End If
          
   
        End If
    End If
   
    paths = ""
    loc = ""


'Response.Write("<br />" & Request.Form("do"))

 %>
 <body>

    <div class="container" style="Border:1px solid #2354fc">
        <div class="row" style="background-color:#2334fa;font-color:#fffff">
                <div class="col-lg-12 col-sm-12 col-md-12" style="background-color:2334fa;font-color:white"><h1>Daily Schedule</h1></div>
            </div>

            <div class="row">
          
                    
                                                              
               <div class="col-sm-6 col-md-6">     
               <div class="form-group data-custon-pick data-custom-mg">
                                        <label>Range select</label>
                                        <div class="input-daterange input-group" id="">
                                           <input class="form-control" id="sch_id" name="sch_id" value="<%=getkey(8) %>"/>
                                        </div>
                                    </div>
                            </div>

          
        </div>
        <div class="row">
        <div class='col-sm-6 col-md-6'>     
<div class='form-group data-custon-pick data-custom-mg'>
 <label>sch_id</label>
 <div class='input-daterange input-group' id='Div1'>
 <input class='form-control' id='Text1' name='sch_id' value=''/>
 </div>
</div> 
</div>
   <div class='form-group-inner'><div class='col-sm-6 col-md-6'>     
<div class='form-group data-custon-pick data-custom-mg'>
 <label>sch_by</label>
 <div class='input-daterange input-group' id='Div2'>
 <input class='form-control' id='sch_by' name='sch_by' value=''/>
 </div>
</div> 
</div>
   <div class='form-group-inner'><div class='col-sm-6 col-md-6'>     
<div class='form-group data-custon-pick data-custom-mg'>
 <label>sch_title</label>
 <div class='input-daterange input-group' id='Div3'>
 <input class='form-control' id='sch_title' name='sch_title' value=''/>
 </div>
</div> 
</div>
   <div class='form-group-inner'> <div class='col-sm-6 col-md-6'>
<label class='label-2'>sch_date_time</label>
   <div class='input-group date' id='Div10'>
  <input type='text' class='form-control' id='sch_date_time' name='sch_date_time'>
   <span class='input-group-addon'>
      <span class='glyphicon glyphicon-calendar'></span>
  </span>
  </div>
  </div>
  <div class='form-group-inner'> <div class='col-sm-6 col-md-6'>
<label class='label-2'>sch_date_time_end</label>
   <div class='input-group date' id='Div5'>
  <input type='text' class='form-control' id='sch_date_time_end' name='sch_date_time_end'>
   <span class='input-group-addon'>
      <span class='glyphicon glyphicon-calendar'></span>
  </span>
  </div>
  </div>

  <div class='form-group-inner'><div class='col-sm-6 col-md-6'>     
<div class='form-group data-custon-pick data-custom-mg'>
 <label>sch_tasks</label>
 <div class='input-daterange input-group' id='Div6'>
 <input class='form-control' id='sch_tasks' name='sch_tasks' value=''/>
 </div>
</div> 
</div>
   <div class='form-group-inner'><div class='col-sm-6 col-md-6'>     
<div class='form-group data-custon-pick data-custom-mg'>
 <label>remark</label>
 <div class='input-daterange input-group' id='Div7'>
 <input class='form-control' id='remark' name='remark' value=''/>
 </div>
</div> 
</div>
   <div class='form-group-inner'> <div class='col-sm-6 col-md-6'>
<label class='label-2'>sys_date_time</label>
   <div class='input-group date' id='Div8'>
  <input type='text' class='form-control' id='sys_date_time' name='sys_date_time'>
   <span class='input-group-addon'>
      <span class='glyphicon glyphicon-calendar'></span>
  </span>
  </div>
  </div>

  <div class='form-group-inner'><div class='col-sm-6 col-md-6'>     
<div class='form-group data-custon-pick data-custom-mg'>
 <label>status</label>
 <div class='input-daterange input-group' id='Div9'>
 <input class='form-control' id='status' name='status' value=''/>
 </div>
</div> 
</div>
</div><span>&nbsp;</span><input type='button' name='btnSave' id='btnSave' value='Save' />
        
        </div>
    </div>
  

  </div>
   
 <div id="listx" style=" width:70%; padding:0px 7px 0px 0px; float:left">
    <%  Dim db As New dbclass
        Dim dt As DataTableReader
        Dim floc As String
        
        '  Dim loc As String
        If keyp = "update" Then
          
      dt = db.dtmake("new" & Today.ToLocalTime, "select * from cm_daily_sch where id=" & Request.QueryString("id"), session("conx"))
      If dt.HasRows = True Then
          dt.Read()
                Response.Write("<script type='text/javascript'>")
                Response.Write("$('#date_return').css({visibility:'visible'});")

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
                   
                Next
                    Response.Write("$('#btnSave').attr('title','update');$('#btnSave').attr('value','Update');</script>")
                End If
                dt.Close()
            End If
           
        Dim mk As New formMaker
        Dim row As String
      
        row = ""
        floc = Server.MapPath("employee") & "/" & Session("emp_id") & "/leave"
        loc = Request.ServerVariables("url").Substring(1, Request.ServerVariables("url").Length - 1)
                    Dim sqlx As String=""
        Try
            sqlx = "select sch_title,sch_date_time,sch_date_time_end,sch_task,status,id from cm_daily_sch where sch_by='" & Session("username") & "' order by id desc"
            row = mk.edit_del_list3("cm_daily_sch", sqlx, "Title,Started,End,Task,Status,", Session("conx"), loc, floc, True, True, True, True)
            Response.Write(row)
        Catch ex As Exception
        Response.Write(ex.ToString & "<br>" & sqlx)
        End Try
      
                    
        'files uploaded
    
    %>
 </div>
 <%
     Dim f As New file_list
     loc = Request.ServerVariables("url").Substring(1, Request.ServerVariables("url").Length - 1)
   '  Response.Write(filesview(Server.MapPath("employee"), Session("emp_id"), "leave", "employee"))%>
 <div></div>
 <div style=" float:left; width:30%;">
 <script type="text/javascript">
    function onchangebb()
    {
        var str=$("#upload").formSerialize();
        //alert("jqueryupdate.aspx?empid=<% response.write(session("emp_id")) %>&" + str + "&dest=leavetake&ftype=jpg,gif,png,doc,docx,pdf&task=upload");
     //  $("#newpage").load("jqueryupdate.aspx?empid=<% response.write(session("emp_id")) %>&" + str + "&dest=leavetake&ftype=jpg,gif,png,doc,docx,pdf&what_task=upload");
      }
 </script>

 </div>
 <div id="print1" style=" float:right; width:59px; height:33px; color:Gray;" onclick="javascirpt:print('listx','','','');">print</div>
 <div style=" clear:both;"></div>
<div id="newpage"></div>
<script type="text/javascript" language="javascript">
//hform=findh(document.getElementById("middle_bar"));
  //  $('.titlet').text("Leave Request");
    //showobjar("formx","titlet",22,2);
  // $( "#messagebox" ).text("<% response.write(Request.QueryString("msg")) %>");
  </script>
  
    
   <form id="frmup" action="" method="post" enctype="multipart/form-data">
    </form>
   
   <%  ' Response.Write(keyp)
       If keyp = "delete" Then
           Dim fs As New file_list
           Dim con As String
           Dim str As String
           con = "<span style='color:red;'> This row of data will not be come again.<br />Are you sure you want delete it?<br /><hr>" & _
           "<img src='images/gif/btn_delete.gif' style='cursor:pointer;' onclick=" & Chr(34) & "javascript:delx('" & idx & "','yes','del123');" & Chr(34) & "></span>"
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
           <div id="dialog-modal" title="Caution"><% Response.Write(con)%>
               
     </div>
           <script type="text/javascript">

               //$( "#dialog:ui-dialog" ).dialog( "destroy" );

               $("#dialog-modal").dialog({
                   resizable: true,
                   modal: true
               });
		
           </script>
           <%  fs = Nothing
           End If

           If flg = 1 Then%>
    <script type="text/javascript">
        //$(document).delay(80000);
        $('#frmx').attr("target","workarea");
        $('#frmx').attr("action","<% response.write(Request.ServerVariables("URL")) %>?msg=<% response.write(msg) %>" );
        $('#frmx').submit();
    </script>
   <%  ElseIf flg = 2 Then
       %><script type="text/javascript">
             showMessage('<%response.write(msg) %>', 'date_taken_from');
       </script>
       <%
   End If
       f = Nothing
       
       
       
       %>
      
    <!-- tawk chat JS
		============================================ -->
   
    <script type="text/javascript">
        function delx(val, ans, hd) {

            if (ans == "yes") {
                // alert(val + ans);
                $('#frmx').attr("target", "_self");
                $('#frmx').attr("action", "<% response.write(loc) %>?task=delete&id=" + val + "&tbl=cm_daily_sch");
                $('#frmx').submit();
            }
            else {
                ha(hd);
            }
        }
   </script>
<%
   
    
    
    
    %>
    <script type="text/javascript">        var prv;
        prv = "";
        var id;
        var focused = "";
        var requf = ["sch_id", "sch_by", "sch_date_time", "sch_date_time_end", "sch_tasks", "remark", "sys_date_time", "status", "x"];
        var fieldlist = ["sch_id", "sch_by", "sch_date_time", "sch_date_time_end", "sch_tasks", "remark", "sys_date_time", "status", "x"];
        function validation1() {
            if ($('#sch_id').val() == '') { showMessage('sch_id cannot be empty', 'sch_id'); $('#sch_id').focus(); return false; }
            if ($('#sch_by').val() == '') { showMessage('sch_by cannot be empty', 'sch_by'); $('#sch_by').focus(); return false; }
            if ($('#sch_date_time').val() == '') { showMessage('sch_date_time cannot be empty', 'sch_date_time'); $('#sch_date_time').focus(); return false; }
            if ($('#sch_date_time_end').val() == '') { showMessage('sch_date_time_end cannot be empty', 'sch_date_time_end'); $('#sch_date_time_end').focus(); return false; }
            if ($('#sch_tasks').val() == '') { showMessage('sch_tasks cannot be empty', 'sch_tasks'); $('#sch_tasks').focus(); return false; }
            if ($('#remark').val() == '') { showMessage('remark cannot be empty', 'remark'); $('#remark').focus(); return false; }
            if ($('#sys_date_time').val() == '') { showMessage('sys_date_time cannot be empty', 'sys_date_time'); $('#sys_date_time').focus(); return false; }
            if ($('#status').val() == '') { showMessage('status cannot be empty', 'status'); $('#status').focus(); return false; }
            else if (focused == "") {
                var ans;
                ans = checkblur();
                if (ans != true) {
                    $("#" + ans).focus();
                } else {
                    var str = $("#frmcm_daily_sch").formSerialize();
                    $("#frmcm_daily_sch").attr("action", "?tbl=cm_daily_sch&task=<% response.write(keyp) %>&key=id&keyval=<% response.write(idx) %>&" + str);
                    $("#frmcm_daily_sch").submit();
                    return true;
                }
            }
        } </script>
 
    <script type="text/javascript">
        $(function () {
            $('#Div10').datetimepicker();
            $("#Div8").datetimepicker();
            $("#Div5").datetimepicker();
        });
        </script>
</body>
</html>
<script type="text/javascript">
   $(document).ready(function() {
  
   });
</script>