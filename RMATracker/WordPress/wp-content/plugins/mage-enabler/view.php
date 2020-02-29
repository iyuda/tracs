<link media="all" type="text/css" href="<?php echo plugins_url( 'default.css', __FILE__ ); ?>" rel="stylesheet">
<form method="post" action="options.php"><div id="mage-enabler-body">
	<div class="mage-enabler-box">
		<div id="mage-enabler-header-box">
			<div id="mage-enabler-header-title">
				<div id="icon-plugins" class="icon32"></div>
				<div id="header-text">Mage Enabler 1.2.2</div>
			</div>
			<div id="mage-enabler-header-top-links"><a href="http://mysillypointofview.richardferaro.com/2010/05/11/mage-enabler/">Support</a> | <a href="http://www.twitter.com/richardferaro">Follow author on Twitter</a> | <a href="http://mysillypointofview.richardferaro.com/">Author's Blog</a></div>
		</div>
		<div class="separator"><p>A plugin which enables Magento's session to run within WordPress</p></div>
		<div class="content with-version"><div id="logo"><img src="<?php echo plugins_url( 'mage-enabler-logo.png', __FILE__ ); ?>" /></div>
		<h3>Samples & Related Posts</h3>
		<?php 
		$mage_enabler_feeds = mage_enabler_get_feed();
		$mage_enabler_items = $mage_enabler_feeds->channel->item;
		$mage_enabler_items_count = count($mage_enabler_items);
		
		$i = 1;
		foreach($mage_enabler_items as $key => $value){
			echo "<li><a href=\"".$value->link."\">".$value->title."</a></li>";

			// Show only first 5 items		
			if($i == 5){ break; }else{ ++$i; }
		}
		
		if($mage_enabler_items_count > 5){ 
			echo "<a href=\"http://mysillypointofview.richardferaro.com/?s=".urlencode('Magento')."\" target=\"_blank\">view more results</a>";
		}
		
		?>
		
		</div>
	</div>
	
	<div class="mage-enabler-box">
		<div class="separator"><p><strong>Configuration Options</strong></p></div>
		
		<div class="content">			
				<?php settings_fields( 'mage-enabler-settings' ); ?>
				<table class="form-table">
					<tr valign="top">
					<th scope="row">Mage.php address
						<div><strong>Absolute URL of the file.</strong> Examples are:<br />Magento installed in the web root
						<code>/home/&lt;username&gt;/public_html/app/Mage.php</code>
						or in a directory within web root
						<code>/home/&lt;username&gt;/public_html/&lt;magento-directory&gt;/app/Mage.php</code>
						</div>
					</th>
					<td><input type="text" name="mage_php_url" value="<?php echo get_option( 'mage_php_url' ); ?>" class="regular-text code" /> 
						<span class="description"><?php echo $message; ?></span></td>
					</tr>
					<?php if( class_exists('Mage') ) { ?>
					<tr valign="top">
					<th scope="row">Default store (auto-detect)
						<div>Store configuration to be used whenever a store is not defined in your script.</div>	
					</th>
					<td><strong><?php echo Mage::app()->getWebsite()->getName()." </strong>&raquo;<strong> ".Mage::app()->getStore()->getName()." (".Mage::app()->getStore()->getCode().")"; ?></strong></td>
					</tr>
					<?php } ?>
				</table>
		</div>
		
	</div>
	<div>
		<p class="submit"><input type="submit" class="button-primary" value="<?php _e( 'Save Changes' ) ?>" /></p>
	</div>
</div></form>