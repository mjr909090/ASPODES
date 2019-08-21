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

    //Ĭ��Toolbar
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

    //�Ƿ�����ı��С plugins/resize/plugin.js
	config.resize_enabled = true;

    //�ı��С�����߶� plugins/resize/plugin.js
	config.resize_maxHeight = 3000;

    //�ı��С����С�߶� plugins/resize/plugin.js
	config.resize_minHeight = 250;

    ////�ı��С������� plugins/resize/plugin.js
	//config.resize_maxWidth = 3000;

    ////�ı��С����С��� plugins/resize/plugin.js
    //config.resize_minWidth = 750;

	config.image_previewText = ' ';//���imageͼƬ���������Ӣ��
	config.filebrowserImageUploadUrl = "/api/Announcement/inst"; //����Ҫ�ϴ���action��servlet
};
