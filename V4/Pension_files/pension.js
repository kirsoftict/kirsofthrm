$( document ).ready( function(){
	
	var suffix_list = new Array( "-a", "-b", "-c", "-d", "-e", "-f", "-g", "-h", "-i", "-j", "-k", "-l", "-m", "-n", "-o", "-p", "-q", "-r", "-s", "-t", "-u", "-v", "-w", "-x", "-y", "-z" );
	var added_page_count = 2, deleted_page_count = 0;
	
	function setupPage(){
		// Set the fields with 'date' class to show Ethiopian Calendar Picker on focus
		$.calendars.picker.setDefaults( {dateFormat: 'MM d, yyyy'} );
		$( '.date' ).calendarsPicker( {calendar: $.calendars.instance('ethiopian', 'am')} );
		/*$.calendars.picker.setDefaults( {dateFormat: 'M d, yy'} );
		$( '.date_short' ).calendarsPicker( {calendar: $.calendars.instance('ethiopian', 'am')} );*/
		
		// Set the month dropdowns to the previous Ethiopian calendar month
		etDate = $.calendars.instance( 'ethiopian', 'am' );
		$( '.month' ).prop( 'selectedIndex', etDate.newDate().monthOfYear() - 2 );
		$( '.print_month' ).html( $( '.month option:selected:first' ).text() );
		// and set the year to the year of the previous month
		$( '.year' ).html( etDate.newDate().add( -1, 'm' ).year() );
	}
	
	setupPage();
	
	$( ".a4_landscape:first .ethiopic" ).jGeez();
	$( ".a4_landscape:last .ethiopic" ).jGeez();
	
	function updatePageNumbers(){
		$( '.page_count' ).html( added_page_count );
	}
	
	$( '#add_page' ).click( function(){
		$.ajax({
			type: "GET",
			url: "../form_page_generator.php",
			data: "action=add_pension_page&page=" + added_page_count + "&offset=" + deleted_page_count,
			success: function( msg ){
				if( msg != '' ){
					$( '.close_btn' ).remove();
					$( '#more_pages' ).append( msg );
					added_page_count++;
					updatePageNumbers();
					setupPage();
					// Select the .ethiopic input elements just added and make them amharic inputable
					$( '.a4_landscape' ).last().find( ".ethiopic" ).jGeez();
					// Scroll down to the just added page
					$( 'html, body, .content' ).animate( {scrollTop: $(document).height()}, 2000 );
				}
			}
		});
		return false;
	});
	
	$( '#content' ).on( "focusout", ".calc", function(){
		var suffix = $(this).attr( 'data' );
		suffix == '-a' ? rows = 6 : rows = 20;
		var count = 0, salary_total = 0, employee_contr_total = 0, employer_contr_total = 0, all_contrib_total = 0;
		var salaried = 0, total_salary = 0, total_employer_contrib = 0, total_employee_contrib = 0, total_contrib = 0;
		for( i = 1; i <= rows; i++ ){
			if( $( '#salary' + suffix + i ).asNumber() > 0 ){
				$( '#employee_contrib' + suffix + i ).val( $( '#salary' + suffix + i ).asNumber() * .07 ).formatCurrency( {symbol:''} );
				$( '#employer_contrib' + suffix + i ).val( $( '#salary' + suffix + i ).asNumber() * .11 ).formatCurrency( {symbol:''} );
				$( '#total_contrib' + suffix + i ).val( $( '#salary' + suffix + i ).asNumber() * .18 ).formatCurrency( {symbol:''} );
				salary_total = salary_total + $( '#salary' + suffix + i ).asNumber();
				employee_contr_total = employee_contr_total + $( '#employee_contrib' + suffix + i ).asNumber();
				employer_contr_total = employer_contr_total + $( '#employer_contrib' + suffix + i ).asNumber();
				all_contrib_total = all_contrib_total + $( '#total_contrib' + suffix + i ).asNumber();
				count++;
				// Since there is data in this row, make the row number visible (by making text color black (#FFF) via css)
				$( '#row_no' + suffix + i ).css( 'color', '#000' );
			}else{
				$( '#employee_contrib' + suffix + i ).val( '' );
				$( '#employer_contrib' + suffix + i ).val( '' );
				$( '#total_contrib' + suffix + i ).val( '' );
				// Since there is no data in this row, hide the row number (by making text color white(#FFF) via css)
				$( '#row_no' + suffix + i ).css( 'color', '#fff' );
			}
		}
		$(this).formatCurrency( {symbol:''} );
		// Do a total of the items on this page
		$( '#total_salary' + suffix ).val( salary_total ).formatCurrency( {symbol:''} );
		$( '#total_employee_contrib' + suffix ).val( employee_contr_total ).formatCurrency( {symbol:''} );
		$( '#total_employer_contrib' + suffix ).val( employer_contr_total ).formatCurrency( {symbol:''} );
		$( '#total_contrib' + suffix ).val( all_contrib_total ).formatCurrency( {symbol:''} );
		$( '#count' + suffix ).val( count ).asNumber();
		
		// Do a summary count to be used to update the 'ክፍል - 3 የወሩ የተጠቃለለ ሂሳብ' section found on the first page
		for( i = 0; i < added_page_count; i++ ){
			salaried = salaried + $( '#count' + suffix_list[ i ] ).asNumber();
			total_salary = total_salary + $( '#total_salary' + suffix_list[ i ] ).asNumber();
			total_employee_contrib = total_employee_contrib + $( '#total_employee_contrib' + suffix_list[ i ] ).asNumber();
			total_employer_contrib = total_employer_contrib + $( '#total_employer_contrib' + suffix_list[ i ] ).asNumber();
			total_contrib = total_contrib + $( '#total_contrib' + suffix_list[ i ] ).asNumber(); 
		}
		// Update the 'ሌሎች አባሪ ግፅዎች ድምር' fields found on the first page
		$( '#other_pages_total_salary-a' ).val( total_salary - $( '#total_salary-a' ).asNumber() ).formatCurrency( {symbol:''} );
		$( '#other_pages_total_employee_contrib-a' ).val( total_employee_contrib - $( '#total_employee_contrib-a' ).asNumber() ).formatCurrency( {symbol:''} );
		$( '#other_pages_total_employer_contrib-a' ).val( total_employer_contrib - $( '#total_employer_contrib-a' ).asNumber() ).formatCurrency( {symbol:''} );
		$( '#other_pages_total_contrib-a' ).val( total_contrib - $( '#total_contrib-a' ).asNumber() ).formatCurrency( {symbol:''} );
		
		// Update the 'ክፍል - 3 የወሩ የተጠቃለለ ሂሳብ' section found on the first page
		$( '#months_salaried' ).val( salaried ).asNumber();
		$( '#months_salary' ).val( total_salary ).formatCurrency( {symbol:''} );
		$( '#months_employee_contrib' ).val( total_employee_contrib ).formatCurrency( {symbol:''} );
		$( '#months_employer_contrib' ).val( total_employer_contrib ).formatCurrency( {symbol:''} );
		$( '#months_total_contrib' ).val( total_contrib ).formatCurrency( {symbol:''} );
	});
	
	$( '#content' ).on( "focusout", ".replicate", function(){
		value = $(this).val();
		name = $(this).attr( 'name' );
		if( $.trim( value ) != '' ){
			for( i = 0; i < added_page_count; i++ ){
				$( '#' + name + suffix_list[ i ] ).val( value );
			}
		}
	});
	
	$( '#content' ).on( "change", "select.screen", function(){
		// Function that copies the value of the drop down (hidden during print) that was
		// just changed to the hidden span that will be made visible during printing
		$( '#' + $(this).attr( 'shadow' ) ).html( $(this).val() );
	});
	
	$( document ).on( "change", ".lang_switcher", function(){
		if( $(this).val() == "en" ){
			$(this).parent().parent().find( ".ethiopic" ).jGeez( "changeoptions", { "enabled": false });
		}else{
			$(this).parent().parent().find( ".ethiopic" ).jGeez( "changeoptions", { "enabled": true });
		}
	});
	
	$( document ).on( "change", ".cal_switcher", function(){
		$.calendars.picker.setDefaults( {dateFormat: 'MM d, yyyy'} );
		if( $(this).val() == "greg" ){
			$(this).parent().parent().find( '.date' ).calendarsPicker( 'option', {calendar: $.calendars.instance('gregorian')} ); 
		}else{
			$(this).parent().parent().find( '.date' ).calendarsPicker( 'option', {calendar: $.calendars.instance( 'ethiopian', 'am' )} );
		}
	});
	
	$( '#print' ).click( function(){
		window.print();
	});
	
	$( document ).on( "click", ".close_btn", function(){
		//$(this).parent().css( 'display', 'none' );
		$(this).parent().remove(); // Delete the one page containing div
		added_page_count--;
		updatePageNumbers();
		// Add a delete button to the page preceeding the page you just deleted. But, you can't delete the first page!
		if( added_page_count >= 2 ){
			$( '.a4_landscape' ).last().before( "<a class='close_btn'>close</a>" );
		}
		// Trigger a "focusout" event on a .calc cell so that values are re-calculated
		$( '#salary-a1' ).trigger( 'focusout' );
	});
	
});