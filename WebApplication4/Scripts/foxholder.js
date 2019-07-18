

jQuery.fn.foxholder = function (number) {
  
  this.addClass("form-container").attr("id", "example-"+number.demo);

  //adding labels with placeholders content. Removing placeholders
  this.find('form').find('input,textarea').each(function() {
    var placeholderText, formItemId, inputType;

    //wrapping form elements in their oun <div> tags
    jQuery(this).wrap('<div class="form-item-block"></div>');

    //creating labels
    inputType = jQuery(this).attr('type');

    if (inputType == 'hidden') {

    } else {
      placeholderText = jQuery(this).attr('placeholder');
      formItemId = jQuery(this).attr('id')
      jQuery(this).after('<label for="'+ formItemId +'"><span>'+ placeholderText +'</span></label>');
      jQuery(this).removeAttr('placeholder');
    }
  });

  //adding class on blur
  jQuery('.form-container form').find('input,textarea').blur(function(){
    if (jQuery.trim(jQuery(this).val())!="") {
      jQuery(this).addClass("active");
    } else {
      jQuery(this).removeClass("active");
    }
  });

  //adding line-height for block with textarea
  jQuery('.form-item-block').each(function() {
    if (jQuery(this).has('textarea').length > 0) {
      jQuery(this).css({'line-height': '0px'});
    }
  });


  //examples scripts

  if (number.demo == 2) {

    //example-2 adding top property for label
    jQuery('#example-2 input, #example-2 textarea').focus(function() {
      var labelTop;
      labelTop = parseInt(jQuery(this).css('padding-top'));
      jQuery(this).next('label').css({'top': 0 - (labelTop + 6)});
      console.log(labelTop);
    });

    jQuery('#example-2 input, #example-2 textarea').blur(function() {
      if (jQuery(this).hasClass('active')) {
      } else {
        jQuery(this).next('label').css({'top': 0});
      }
    });
  }
}





