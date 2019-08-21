/**
 * @license Copyright (c) 2003-2016, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here.
	// For complete reference see:
	// http://docs.ckeditor.com/#!/api/CKEDITOR.config

	// The toolbar groups arrangement, optimized for two toolbar rows.
	//config.toolbarGroups = [
	//	{ name: 'clipboard',   groups: [ 'clipboard', 'undo' ] },
	//	{ name: 'editing',     groups: [ 'find', 'selection', 'spellchecker' ] },
	//	{ name: 'links' },
	//	{ name: 'insert' },
	//	{ name: 'forms' },
	//	{ name: 'tools' },
	//	{ name: 'document',	   groups: [ 'mode', 'document', 'doctools' ] },
	//	{ name: 'others' },
	//	'/',
	//	{ name: 'basicstyles', groups: [ 'basicstyles', 'cleanup' ] },
	//	{ name: 'paragraph',   groups: [ 'list', 'indent', 'blocks', 'align', 'bidi' ] },
	//	{ name: 'styles' },
	//	{ name: 'colors' },
	//	{ name: 'about' }
	//];

    //默认Toolbar
    config.toolbar = 'MyToolbar';

    config.toolbar_MyToolbar =
    [
        ['Undo', 'Redo', '-', 'SelectAll', 'RemoveFormat'],
        ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', 'Blockquote', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
        ['Maximize'],        
        ['Format', 'Font', 'FontSize'],
        ['TextColor', 'BGColor'],
        ['Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript']
    ];

	// Remove some buttons provided by the standard plugins, which are
	// not needed in the Standard(s) toolbar.
	config.removeButtons = 'Underline,Subscript,Superscript';

	// Set the most common block elements.
	config.format_tags = 'p;h1;h2;h3;pre';

	// Simplify the dialog windows.
	config.removeDialogTabs = 'image:advanced;link:advanced';

    //是否允许改变大小 plugins/resize/plugin.js
	config.resize_enabled = true;

    //改变大小的最大高度 plugins/resize/plugin.js
	config.resize_maxHeight = 3000;

    //改变大小的最小高度 plugins/resize/plugin.js
	config.resize_minHeight = 250;

    ////改变大小的最大宽度 plugins/resize/plugin.js
	//config.resize_maxWidth = 3000;

    ////改变大小的最小宽度 plugins/resize/plugin.js
    //config.resize_minWidth = 750;

	config.image_previewText = ' ';//清空image图片里面的无用英文
	config.filebrowserImageUploadUrl = "/api/Announcement/inst"; //待会要上传的action或servlet
};
