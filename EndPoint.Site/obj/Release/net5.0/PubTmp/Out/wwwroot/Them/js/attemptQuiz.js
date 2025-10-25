$(document).ready(function () {
    var startBtn = document.getElementById('start');

    var questionsCount = 0;
    var counter = 1;


    if (startBtn) {

        var forms = document.getElementsByTagName('form');
        var form;
        if (forms.length > 1) {
            form = forms[1]
        } else {
            form = forms[0]
        }
        //  questionsCount = parseInt(form.id);
        questionsCount = $('.form_question').attr("data-formcount");
        //console.log("loadQuestion_questionsCount", parseInt(form.id));
        //console.log("loadFFForm_questionsCount", questionsCount);
        var nextBtns = Array.from(document.getElementsByTagName('a')).filter(x => x.id.includes('next'));
        var prevBtns = Array.from(document.getElementsByTagName('a')).filter(x => x.id.includes('prev'));
        $(nextBtns).click(loadNextQuestion);
        $(prevBtns).click(loadPreviousQuestion);
        startQuizEventHandler()
    }

    function startQuizEventHandler(mins) {
        $(startBtn).click(function () {
            $(window).bind('beforeunload', function () {
                return 'آیا واقعا می خواهید آزمون را ترک کنید ؟?';
            });

            $('#submitResult').click(function () {
                $(window).unbind('beforeunload');
            });

            ajaxSendQuizId(mins);

            //if (mins) {
            //    $('#clockdiv').show();
            //    startTimer();
            //}
            //$('#pagging').show();
            //$('#submit').show();
            //$('#details').hide();
            //showQuestion(counter);
            //var pagginationBtns = [...document.getElementsByClassName('page-item number')];
            //pagginationBtns.forEach(x => $(x).click(loadQuestion));
            //$('#first').click(loadPreviousQuestion);
            //$('#last').click(loadNextQuestion);
        })
    }

    function ajaxSendQuizId() {
        var id = $('#quizId').val();
$('#overlay').fadeIn(300);
        var url = '/Student/Quizzes/StartedQuizAjaxCall';


        $.ajax(url, {
            type: 'POST',  // http method
            data: { password: $('#hiddenkey').attr("data-pass"), quizid: $('#hiddenkey').attr("data-quizid") },
            success: function (result, status, xhr) {
                $('#overlay').fadeOut(300);
                if (result.isSuccess) {
                    $("#loadquizzz").empty();
                    $("#loadquizzz").html(result.data);

                    $('#pagging').show();
                    $('#submit').show();
                    $('#details').hide();
                    showQuestion(counter);
                    var pagginationBtns = [...document.getElementsByClassName('page-item number')];
                    pagginationBtns.forEach(x => $(x).click(loadQuestion));
                    $('#first').click(loadPreviousQuestion);
                    $('#last').click(loadNextQuestion);


                    var forms = document.getElementsByTagName('form');
                    var form;
                    if (forms.length > 1) {
                        form = forms[1]
                    } else {
                        form = forms[0]
                    }
                    var nextBtns = Array.from(document.getElementsByTagName('a')).filter(x => x.id.includes('next'));
                    var prevBtns = Array.from(document.getElementsByTagName('a')).filter(x => x.id.includes('prev'));
                    $(nextBtns).click(loadNextQuestion);
                    $(prevBtns).click(loadPreviousQuestion);
                    var minsInput = document.getElementById("minutes");
                    var mins = null;

                    if (minsInput) {

                        mins = minsInput.value;
                    }
                    if (mins) {
                        $('#clockdiv').show();
                        startTimer(mins);
                    }
                }
                else {
                    var delay = 3000;
                    Swal.fire({
                        icon: 'warning',
                        title: result.message,
                        showConfirmButton: false,
                        timer: delay
                    })
                    setTimeout(function () { window.location = "/Student/Quizzes/Index"; }, delay);
                }

            },
            error: function (jqXhr, textStatus, errorMessage) {
                $('#overlay').fadeOut(300);
            }
        });
    }

    function loadQuestion(e) {
        e.preventDefault();
        hideQuestion(counter)
        counter = parseInt(e.currentTarget.classList[e.currentTarget.classList.length - 1]);
        showQuestion(counter);
    }

    function loadNextQuestion(e) {
        e.preventDefault();
        //console.log("loadNextQuestion_FirstFunc", counter);

        hideQuestion(counter)
        if (counter == questionsCount) {
           // console.log("loadLasssssssssstQuestion", counter);
            showQuestion(counter);

        }
        else {
            showQuestion(counter + 1);
          //  console.log("load_kochektar_Question", counter);
        }

        if (counter < questionsCount) {
            counter++;
        }

       // console.log("loadNextQuestion_EndFunc", counter);
    }

    function loadPreviousQuestion(e) {
        e.preventDefault();
       // console.log("loadPreviousQuestion", counter);
        hideQuestion(counter);
        if (counter == 1) {
            showQuestion(counter);
        } else {
            showQuestion(counter - 1)
        }

        if (counter > 1) {
            counter--;
        }
    }

    function showQuestion(counter) {
       // console.log("showQuestion", counter);
       // $(`#${counter}`).show();
        $(`div[data-quizid=${counter}]`).show();

       
        if (counter == 1) {
            $('#first').addClass('disabled');
        } else if (counter == questionsCount) {
            $('#last').addClass('disabled');
        }
        else {
            $('#first').removeClass('disabled');
            $('#last').removeClass('disabled');
        }
        $('.number').removeClass('active');
        $(`.${counter}`).addClass('active');
    }

    function hideQuestion(counter) {
       // $(`#${counter}`).hide();
      // $(`div[data-quizid=${counter}]`).hide();
       if (counter <= questionsCount) {
            $(`div[data-quizid=${counter}]`).hide();
        }
      
    }

    function startTimer(mins) {
        let now = new Date($.now());
        let endTime = getEndDate(now, mins);
        initializeClock('clockdiv', endTime);

        function getTimeRemaining(endtime) {
            var t = Date.parse(endtime) - Date.parse(new Date());
            var seconds = Math.floor((t / 1000) % 60);
            var minutes = Math.floor((t / 1000 / 60) % 60);

            return {
                'total': t,
                'minutes': minutes,
                'seconds': seconds
            };
        }

        function initializeClock(id, endtime) {
            var clock = document.getElementById(id);
            var minutesSpan = clock.querySelector('.minutes');
            var secondsSpan = clock.querySelector('.seconds');

            function updateClock() {
                var t = getTimeRemaining(endtime);

                minutesSpan.innerHTML = ('0' + t.minutes).slice(-2);
                secondsSpan.innerHTML = ('0' + t.seconds).slice(-2);

                if (t.total <= 0) {
                    clearInterval(timeinterval);
                    $(window).unbind('beforeunload');
                    $('#submitResult').click();
                }
            }

            updateClock();
            var timeinterval = setInterval(updateClock, 1000);
        }

        function getEndDate(dt, minutes) {
            return new Date(dt.getTime() + minutes * 60000).toString();
        }
    }


})
