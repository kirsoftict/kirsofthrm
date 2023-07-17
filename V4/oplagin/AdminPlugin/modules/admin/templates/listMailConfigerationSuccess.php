<script type="text/javascript" src="<?php echo public_path('../../scripts/jquery/jquery.validate.js')?>"></script>
   <div class="formpageNarrow">
        	<div class="outerbox">
        		<div class="top">
        			<div class="left"></div><div class="right"></div><div class="middle"></div>
        		</div>
        		<div class="maincontent">
            <div class="mainHeading"><h2><?php echo __("Subscribe to Email Notifications")?></h2></div>

        
	            <form action="" onsubmit="" method="post" id="frmSave" name="frmSave">
	
	                <input type="hidden" value="UpdateRecord" name="sqlState"/>
	                <label for="txtMailAddress"><?php echo __("Email")?><span class="required">*</span></label>
	                <input type="text"  value="<?php echo $currentUser->getEmail1()?>" class="formInputText" id="txtMailAddress" name="txtMailAddress"/>
	                <br class="clear"/>
	
	                  <label for="txtSkillName"><?php echo __("Leave Applications")?></label>
	           
	            <input type="checkbox"  class="formCheckbox" value="1" name="notificationMessageStatus[]" <?php if(isset($notficationList[1]) && $notficationList[1]==1){ echo "checked='checked'";}?>"/>
	            <br class="clear"/>
	                  <label for="txtSkillName"><?php echo __("Leave Approvals")?></label>
	            
	            <input type="checkbox"  class="formCheckbox" value="2" name="notificationMessageStatus[]" <?php if(isset($notficationList[2]) && $notficationList[2]==1){ echo "checked='checked'";}?>/>
	            <br class="clear"/>
	                  <label for="txtSkillName"><?php echo __("Leave Cancellations")?></label>
	           
	            <input type="checkbox"  class="formCheckbox" value="0" name="notificationMessageStatus[]" <?php if(isset($notficationList[0]) && $notficationList[0]==1){ echo "checked='checked'";}?>/>
	            <br class="clear"/>
	                  <label for="txtSkillName"><?php echo __("Leave Rejections")?></label>
	            
	            <input type="checkbox"  class="formCheckbox" value="-1" name="notificationMessageStatus[]" <?php if(isset($notficationList[-1]) && $notficationList[-1]==1){ echo "checked='checked'";}?>/>
	            <br class="clear"/>
	                  <label for="txtSkillName"><?php echo __("Job Applications")?></label>
	          
	            <input type="checkbox"  class="formCheckbox" value="4" name="notificationMessageStatus[]" <?php if(isset($notficationList[4]) && $notficationList[4]==1){ echo "checked='checked'";}?>/>
	            <br class="clear"/>
	                  <label for="txtSkillName"><?php echo __("New employee hire approval requests")?></label>
	           
	            <input type="checkbox"  class="formCheckbox" value="5" name="notificationMessageStatus[]" <?php if(isset($notficationList[5]) && $notficationList[5]==1){ echo "checked='checked'";}?>/>
	            <br class="clear"/>
	                  <label for="txtSkillName"><?php echo __("Tasks sent on hiring of employee")?></label>
	          
	            <input type="checkbox"  class="formCheckbox" value="6" name="notificationMessageStatus[]" <?php if(isset($notficationList[6]) && $notficationList[6]==1){ echo "checked='checked'";}?>/>
	            <br class="clear"/>
	                  <label for="txtSkillName"><?php echo __("Notifications of hiring new employees")?></label>
	            
	            <input type="checkbox"  class="formCheckbox" value="7" name="notificationMessageStatus[]" <?php if(isset($notficationList[7]) && $notficationList[7]==1){ echo "checked='checked'";}?>/>
	            <br class="clear"/>
	                  <label for="txtSkillName"><?php echo __("HSP Notifications")?></label>
	            
	            <input type="checkbox"  class="formCheckbox" value="1" name="notificationMessageStatus[]" <?php if(isset($notficationList[3]) && $notficationList[3]==1){ echo "checked='checked'";}?>/>
	            <br class="clear"/>
	      
	
	                <div class="formbuttons">
	                    <input type="button" value="<?php echo __("Edit")?>"   id="editBtn" class="editbutton"/>
	                    <input type="button" value="<?php echo __("Reset")?>" id="resetBtn"  tabindex="3"  class="clearbutton"/>
	                </div>
	            </form>
        	</div>
        	<div class="bottom">
        		<div class="left"></div><div class="right"></div><div class="middle"></div>
        	</div>
       
        <div class="requirednotice"><span class="required">*</span> <?php echo __(CommonMessages::REQUIRED_FIELD); ?></div>
    </div>
<script type="text/javascript">
	$(document).ready(function() {

	var mode	=	'edit';
		
		//Disable all fields
		$('#frmSave :input').attr('disabled', true);
		$('#editBtn').removeAttr('disabled');
		
		$("#editBtn").click(function() {
			
			if( mode == 'edit')
			{
				$('#editBtn').attr('value', 'Save'); 
				$('#frmSave :input').removeAttr('disabled');
				mode = 'save';
			}else
			{
				
				$('#frmSave').submit();
			}
		});

		//Validate the form
		 $("#frmSave").validate({
			
			 rules: {
			 	txtMailAddress: { required: true }
		 	 },
		 	 messages: {
		 		txtMailAddress: '<?php echo __(ValidationMessages::REQUIRED); ?>'
		 	 }
		 });

		//When click reset buton 
			$("#resetBtn").click(function() {
				document.forms[0].reset('');
			 });

	 });
</script>