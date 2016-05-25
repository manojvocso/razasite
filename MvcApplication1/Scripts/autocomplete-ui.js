   

                                                        jQuery(function () {
                                                            jQuery('.header-dropkick').dropkick();

                                                            jQuery('.black').dropkick({
                                                                theme: 'black'
                                                            });

                                                            //                                                        jQuery('.change').dropkick({
                                                            //                                                            change: function (value, label) {
                                                            //                                                                alert('You picked: ' + label + ':' + value);
                                                            //                                                            }
                                                            //                                                        });

                                                            jQuery('.existing_event').dropkick(/*{
          change: function () {
          $(this).change(); // This is just duplicating the event, let's remove
        }
      }*/
                                                            );

                                                            jQuery('.custom_theme').dropkick({
                                                                theme: 'black',
                                                                change: function (value, label) {
                                                                    $(this).dropkick('theme', value);
                                                                }
                                                            });

                                                            jQuery('.dk_container').first().focus();
                                                        });

                                                        var jsonData = [];

                                                        var predata = [];
                                                        $.ajax({
                                                            url: "/Account/GetCountryToListFlag/",
                                                            type: 'GET',
                                                            success: function(data) {
                                                                predata = data;

                                                                $.each(data, function() {
                                                                    jsonData.push({ id: this.Id, name: this.CountryFlag, status: 'Already visited' });
                                                                });

                                                                var ms_header = $('#ms-header').magicSuggest({
                                                                    data: jsonData,
                                                                    resultAsString: true,
                                                                    maxSelection: 1,
                                                                    maxSelectionRenderer: function() {
                                                                    }
                                                                });

                                                                var countryfrom = 1;
                                                                jQuery('.header-dropkick').dropkick({
                                                                    change: function(value, label) {
                                                                        //alert('You picked: ' + label + ':' + value);
                                                                        countryfrom = value;
                                                                        //countryfrom = $('#hed-sel-kick').val();
                                                                    }
                                                                });


                                                                $(ms_header).on('selectionchange', function(event, selection) {
                                                                    var selected = ms_header.getSelectedItems();
                                                                    var countryto = selected[0].id;

                                                                    window.location.href = "/Rate/SearchRate?countryfrom=" + countryfrom + "&countryto=" + countryto + "";
                                                                });

                                                            }
                                                        });  
            
                                                    