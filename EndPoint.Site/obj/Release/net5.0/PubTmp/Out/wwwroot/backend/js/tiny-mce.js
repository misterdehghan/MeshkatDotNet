var useDarkMode = window.matchMedia('(prefers-color-scheme: dark)').matches;
tinymce.init({
    selector: "textarea.set-editor",
    language: "fa_IR",
    plugins: "print preview paste searchreplace autolink autosave save directionality code visualblocks " +
        "visualchars image link media template table charmap hr nonbreaking anchor toc " +
        "insertdatetime advlist lists wordcount imagetools textpattern noneditable help charmap quickbars emoticons readmore",
    imagetools_cors_hosts: ["picsum.photos"],
    menubar: 'file edit view insert format tools table help',//fontselect
    toolbar1: 'undo redo | bold italic underline strikethrough | fontsizeselect formatselect | alignleft aligncenter alignright alignjustify | outdent indent |  numlist bullist | forecolor backcolor removeformat | ',
    toolbar2: 'charmap emoticons | preview save print | insertfile insertMultipleImage image media template link anchor code | ltr rtl | readmore hr ',
    setup: function (editor) {
        var toTimeHtml = function (date) {
            var elements = "";
            $.map(date, function(f) {
                elements += `<img src="${f.url}">`;
            });
            return elements;
        };

        //editor.ui.registry.addButton('insertMultipleImage', {
        //    icon: 'gallery',
        //    tooltip: 'افزودن گالری',
        //    disabled: true,
        //    onAction: function (_) {
        //        editor.windowManager.openUrl({
        //            title: "انتخاب فایل",
        //            url: "/Pnl/FileManager/MultipleSelect",
        //            width: 900,
        //            height: 450,
        //            resizable: "yes",
        //            //buttons: [
        //            //    {
        //            //        type: 'custom',
        //            //        name: 'action',
        //            //        text: 'انتخاب',
        //            //        primary: true
        //            //    },
        //            //    {
        //            //        type: 'cancel',
        //            //        name: 'cancel',
        //            //        text: 'بستن'
        //            //    }
        //            //], onAction: function (instance, trigger) {
        //            //    console.log(instance);
        //            //    console.log(trigger);
        //            //    instance.close();
        //            //},
        //            onClose: function () {
        //                editor.insertContent(toTimeHtml(tinymceCallBackFils));
        //            }
        //        });
        //    },
        //    onSetup: function (buttonApi) {
        //        window.tinymceCallBackFils = "";
        //        window.tinymceWindowManager = window.tinymce.activeEditor.windowManager;
        //        var editorEventCallback = function (eventApi) {
        //            buttonApi.setDisabled(eventApi.element.nodeName.toLowerCase() === 'time');
        //        };
        //        editor.on('NodeChange', editorEventCallback);

        //        /* onSetup should always return the unbind handlers */
        //        return function (buttonApi) {
        //            editor.off('NodeChange', editorEventCallback);
        //        };
        //    }
        //});
    },
    toolbar_sticky: false,
    autosave_ask_before_unload: true,
    autosave_interval: '30s',
    autosave_prefix: '{path}{query}-{id}-',
    autosave_restore_when_empty: false,
    autosave_retention: '2m',
    image_advtab: true,
    automatic_uploads: false,
    link_list: [
        { title: 'My page 1', value: 'https://www.saas.net' },
        { title: 'My page 2', value: 'http://www.saas.net' }
    ],
    //image_list: [
    //    { title: 'My page 1', value: 'https://www.tiny.cloud' },
    //    { title: 'My page 2', value: 'http://www.moxiecode.com' }
    //],
    //image_class_list: [
    //    { title: 'None', value: '' },
    //    { title: 'Some class', value: 'class-name' }
    //],
    file_picker_callback: elFinderBrowser,
    relative_urls: false, 
    file_picker_types: 'image media',
    templates: [
        { title: 'New Table', description: 'creates a new table', content: '<div class="mceTmpl"><table width="98%%"  border="0" cellspacing="0" cellpadding="0"><tr><th scope="col"> </th><th scope="col"> </th></tr><tr><td> </td><td> </td></tr></table></div>' },
        { title: 'Starting my story', description: 'A cure for writers block', content: 'Once upon a time...' },
        { title: 'New list with dates', description: 'New List with dates', content: '<div class="mceTmpl"><span class="cdate">cdate</span><br /><span class="mdate">mdate</span><h2>My List</h2><ul><li></li><li></li></ul></div>' }
    ],
    template_cdate_format: '[Date Created (CDATE): %m/%d/%Y : %H:%M:%S]',
    template_mdate_format: '[Date Modified (MDATE): %m/%d/%Y : %H:%M:%S]',
    height: 500,
    image_caption: true,
    quickbars_insert_toolbar: '',
    quickbars_selection_toolbar: 'bold italic | quicklink h2 h3 blockquote | image quicktable',
    noneditable_noneditable_class: 'mceNonEditable',
    toolbar_mode: 'sliding',
    pagebreak_split_block: true,
    contextmenu: 'link image imagetools table',
    content_style: 'body { font-family:Vazir,Arial,sans-serif; font-size:14px }'
});

function elFinderBrowser(callback, value, meta) {
    window.tinymceCallBackURL = "";
    window.tinymceCallBackFm = "";
    window.tinymceWindowManager = window.tinymce.activeEditor.windowManager;

    window.tinymce.activeEditor.windowManager.openUrl({
        title: "انتخاب فایل",
        url: "/Pnl/FileManager/TinyBrowse",
        width: 900,
        height: 450,
        resizable: "yes",
        onClose: function () {
            if (tinymceCallBackURL !== '') {
                // URL normalization
                var url = tinymceCallBackFm.convAbsUrl(tinymceCallBackURL.url);

                // Make file info
                var info = tinymceCallBackURL.name + " (" + tinymceCallBackFm.formatSize(tinymceCallBackURL.size) + ")";

                // Provide file and text for the link dialog
                if (meta.filetype === "file") {
                    callback(url, { text: info, title: info });
                }

                // Provide image and alt text for the image dialog
                if (meta.filetype === "image") {
                    callback(url, { alt: info });
                }

                // Provide alternative source and posted for the media dialog
                if (meta.filetype === "media") {
                    callback(url);
                }
            }
        }

    });
}