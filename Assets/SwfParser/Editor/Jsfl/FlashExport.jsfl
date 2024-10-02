var script_dir = fl.scriptURI.replace('FlashExport.jsfl', '');
fl.runScript(script_dir + 'Internal/FTBase.jsfl');
fl.runScript(script_dir + 'Internal/FTMain.jsfl', "ft_main", ft, {
	graphics_scale : 1.0
	//graphics_scale : 2.0 //HD
	//graphics_scale : 0.5 //SD
});