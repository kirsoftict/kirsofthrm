<%@ Page Language="VB" AutoEventWireup="false" CodeFile="newreport.aspx.vb" Inherits="newreport" %>

<%@ Import Namespace="system.IO" %>
<%@ Import Namespace="system.data" %>
<%@ Import Namespace="system.data.sqlclient" %>
<%@ Import Namespace="Kirsoft.hrm" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"><html xmlns="http://www.w3.org/1999/xhtml">
<head>

	
	<link rel="stylesheet" href="jq/themes/ui-lightness/jquery.ui.all.css" />
	<script type="text/javascript" src="jq/jquery-1.7.2.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.core.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.widget.js"></script>
	<script type="text/javascript" src="jq/ui/jquery.ui.datepicker.js"></script>
	
	<title></title>
    <script type="text/javascript">
   var where=new Array(6); 
   function mksql()
   {
           var strwhere="";
                         where[0]="";
            if($("#empsearch_name").val()!="")
            {   
                where[0]="first_name like '" + $("#empsearch_name").val() + "%'";
            }
            where[1]="";
               if($("#empid").val()!="")
            {   
                where[1]="emp_id like '" + $("#empid").val() + "%'";
            } 
             where[2]="";
            if($("#emptype").val()!="")
            {   
                where[2]="type_recuritment like '" + $("#emptype").val() + "%'";
            }  
            where[3]="";
            if($("#emp_dep").val()!="")
            {   
                where[3]="department like '" + $("#emp_dep").val() + "%'";
            }
            where[4]="";
            if($("#empstate").val()!="")
            {   
                if($("#empstate").val()=="n")
                where[4]="active = '" + $("#empstate").val() + "' and first_name not in(select first_name from vwemp_emp_dep_qual where end_date is null)";
                else
                where[4]="active = '" + $("#empstate").val() + "'";
                
            }where[5]="";
             if($("#empqul").val()!="")
            {   
                where[5]="qualification = '" + $("#empqul").val() + "'";
            }
           var k=0;
           var kw=0;
            for(i=0;i<6;i++)
            {
                if(where[i]!="")
                {  if(k==0)
                    {
                        k=1;
                        strwhere=strwhere+"where ";
                    }
                   
                        
                    
                    strwhere  =  strwhere + where[i]; 
                   strwhere= strwhere + " and "  ; 
                }
                
             }
           var and=strwhere.substr(strwhere.length-4,strwhere.length);
           if(and=="and ")
           {
                strwhere=strwhere.substr(0,strwhere.length-4);
           }
    var sql="select * from vwemp_emp_dep_qual " + strwhere;
  $("#textone").text(sql);
 // alert(sql);
  $("divlist").load("view_report1.aspx?sql=" + sql);
  return true;
    //$('#search_form').attr('target','listview');
     //   $('#search_form').attr('action','listemp.aspx?sql=' + sql); 
     //  $('#search_form').submit();
   }
        function onfocus_click(id)
        {
            //$('#' + id).append(new Option("kir","kir"));
        }
    </script>
</head>
<body style="">
<%  If Session("emp_id") <> "" Then
        Session("emp_id") = Nothing
        Session("fullempname") = Nothing
        Session("emp_path") = Nothing
    End If%>
<div style="">
  <div>
    <div>
      <h2>Employee Information</h2>
    </div>
    <div>
      <form id="search_form" method="post" action="">
        <div id="formcontent"><br />
          <label for="empsearch_employee_name">Employee Name</label>
          <input name="empsearch_name" value="" id="empsearch_name"/>
          <label for="empid">Id</label>
          <input type="text" name="empid" id="empid" />
          <label for="emptype">Employment Type</label>
          <select name="emptype" id="emptype">
             
            <option value="">All</option>
          
<%  Dim fm As New formMaker
    Response.Write(fm.getoption("tbl_emp_type where status='y'", "emptype", "emptype", session("con")))%>

             
          </select>
           <label for="empqul">Employee Qualification</label>
          <select name="empqul" id="empqul">
             
            <option value="">All</option>
          
<%  
    Response.Write(fm.getoption("tblqualification where stat='y'", "qualification", "qualification", session("con")))%>

             
          </select>
          <label for="empstate">Include</label>
          <select name="empstate" id="empstate">
             
            <option value="y">Current Employees Only</option>
             
            <option value="" selected="selected">Current and Past Employees</option>
             
            <option value="n">Past Employees Only</option>
             
          </select>
          <br />
          <label for="emp_dep">Department:</label>
          <select name="emp_dep" id="emp_dep">
             <option value="">All</option>
               <%  
                   Response.Write(fm.getoption("tbldepartment", "dep_id", "dep_name", session("con")))%>

   
             
          </select>
         
          <script type="text/javascript">
          
          </script>
          <div>
            <div>
              <!--input type="button" id="searchBtn" onmouseover="this.className='plainbtn plainbtnhov'" onmouseout="this.className='plainbtn'" value="Search" name="_search" />
              <input type="button" onmouseover="this.className='plainbtn plainbtnhov'" id="resetBtn" onmouseout="this.className='plainbtn'" value="Reset" name="_reset" /-->
            </div>
            <br />
          </div>
          <br />
        </div>
      </form>
    </div>
  </div>
  
</div>

<% 
    If Session("username") = "" Then
        Response.Redirect("logout.aspx")
    End If
  %> 
    <div id="divlist" style="overflow:auto; height:500px; vertical-align:top"></div>
   
    <div id="textone">
    </div>
</body>
</html>
<script language="javascript" type="text/javascript">
 $(document).ready(function(){
  $('#search_form').keyup(function(){
  mksql();
       // var sql="select * from emp_static_info where first_name like '" + $('#empsearch_name').val() + "%'";
        //$('#search_form').attr('target','listview');
        //$('#search_form').attr('action','listemp.aspx?sql=' + sql); 
        //$('#search_form').submit();
        
    });    
   $('#search_form').change(function(){
  mksql();
       // var sql="select * from emp_static_info where first_name like '" + $('#empsearch_name').val() + "%'";
        //$('#search_form').attr('target','listview');
        //$('#search_form').attr('action','listemp.aspx?sql=' + sql); 
        //$('#search_form').submit();
        
    });    
       $(document).height('310');
      
    });
  //$("#divlist").load("listemp.aspx");


</script>
