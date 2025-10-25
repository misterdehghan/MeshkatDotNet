const vm = new Vue({
    el: '#vue_det',
    data: {
        firstname: "کامبیز",
        lastname: "زارع ابراهیمی",
        address: "شهرقدس(قلعه حسن خان)",
        total: '',
        show: true,
        info: null,
        styleobj: {
            backgroundColor: '#2196F3!important',
            cursor: 'pointer',
            padding: '8px 16px',
            verticalAlign: 'middle',
        },
        todos: arr
    },
    computed: {
        // a computed getter
        reversedMessage: function () {
            // `this` points to the vm instance
            return this.lastname.split('').reverse().join('')
        }
    },
    methods: {
        mydetails: function () {
            return this.total = "من " + this.firstname + " " + this.lastname + "اهل " + this.address + "  هستم ";
        },
        displaynumbers: function (a, b, wi, d, s) {
            $('.editAnswerTitle').html("<span> ویرایش جواب برای : </span>" + b);
            $('#modalEditId').val(a);
            $('#modalEditSurveyQuestionId').val(s);
            $('#modalEditTitle').val(b);
            $('#modalEditTitle').text(b);
            $('#modalEditWight').val(wi);
            $('#modalEditWight').text(wi);
            $('#modalEditIndex').val(d);
            $('#modalEditIndex').text(d);
            console.log("test send id", a)
            console.log("test send text", b)
            console.log("test send wi", wi)
            console.log("test send index", d)
            console.log("test send SurveyQuestionId", s)
            $('#myModalAnsweres').modal('toggle');
            $('#myModalAEditAnswer').modal('show');


        }
     
    }
  
})
