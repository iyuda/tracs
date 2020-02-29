=== Mage Enabler ===
Contributors: richardferaro
Donate link: https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=A4YNMXBEXWQWA
Tags: magento, ecommerce, mage, session
Requires at least: 2.9.2
Tested up to: 3.4.2
Stable tag: 1.2.2

A plugin which enables Magento's session to run within WordPress.

== Description ==

This plugin links your Magento installation and WordPress by instantiating the Magento's Mage object. It is done by using PHP `require_once` of the file `Mage.php` supplied by Magento. By doing this, the administrator will be able to pull and push information to/from the Magento. This requires a working Magento setup prior to installation of this plugin. At the moment, Mage object can be used to non-admin pages such as themes.

= Is this plugin for you? =
If you know how to program using PHP with basic knowledge of object oriented programming and familiar with Magento, then yes, this plugin is for you. The plugin, once activated, gives you a raw access to the Mage object which you can use to access Magento methods within WordPress and do stuff like [single login](http://mysillypointofview.richardferaro.com/2010/04/08/how-to-use-magentos-session-within-wordpress/), [pull templates from Magento](http://mysillypointofview.richardferaro.com/2010/07/03/how-to-add-magento-blocks-css-and-javascript-to-an-external-site/), display categories, [products](http://mysillypointofview.richardferaro.com/2010/06/04/add-a-product-with-custom-options-into-magento-cart-from-external-site/) and checkout from your blog and many [more](http://docs.magentocommerce.com/). It's basically extending the ecommerce function of Magento to your WordPress blog.

= This plugin is not for you if: =
* You want to display your WordPress blog within your Magento shop (Go to Magento Connect instead)
* You don't know how to program using PHP
* You have no idea what Magento is

Please do take some time to rate the plugin as well as fill in the information needed in the compatibility section. It would be great if you can share your site (only if you're allowed to do so) which uses this plugin but it is optional though.

== Installation ==

1. Fix the function collision problem between Magento and WordPress by doing the following:

   Copy the file `functions.php` from your Magento core folder which can be found in: 

            /magento/app/code/core/Mage/Core/functions.php 


   and paste it in the directory below (create the directory if necessary) and open the file for editing. 

            /magento/app/code/local/Mage/Core/functions.php


   Locate the `function __()` or go to line 93. The function is similar to the one below:

            function __() 
            { 
                return Mage::app()->getTranslator()->translate(func_get_args()); 
            }


   replace it with this:

            if (!function_exists('__')) {
	            function __()
	            {
		        return Mage::app()->getTranslator()->translate(func_get_args());
	            }
            }


   Save the file and close it.
1. Upload `mage-enabler` folder to the `/wp-content/plugins/` directory or use the `Add New -> Upload` option in the Administration panel.
1. If you use FTP to upload the plugin, activate the plugin through the 'Plugins' menu in WordPress. If you use the `Add New -> Upload` option, just click the Activate link provided after the plugin's installation. A menu will be added under <code>Settings</code> tab labeled as <code>Mage Enabler</code>.
1. Go to the Mage Enabler admin page in WordPress and supply the absolute URL of the file `Mage.php` from Magento
1. Click Save button. One of the messages below will shows upon saving the settings:
   * Invalid URL - This means your file `Mage.php` can't be found using the URL you entered.
   * File is accessible! - You have supplied a valid URL.
   * Mage object not found! - Check your cookie settings. Your browser should be configured to accept cookies.

== Frequently Asked Questions ==

= How to use this plugin? =

Check my blog for an example [here](http://mysillypointofview.richardferaro.com/2010/05/11/mage-enabler/)

= What Magento methods can I use? =

Check the [Magento documentation](http://docs.magentocommerce.com/)

= My website appears blank after I turn on the plugin =

It means there's an error in your code. Set your PHP error_reporting to E_ALL and refresh the page so you can see what's causing the error.

= I already set my PHP error reporting to display all errors in .htaccess but still my page remains blank =

Try placing the <code>error_reporting(E_ALL)</code> without quotes at the top of the actual file you're working on then reload the page.

= I get 'Cannot send session cookie - headers already sent by...' error =

Try placing your <code>Mage::getSingleton(...)</code> call at the top of your PHP file to prevent this error from taking place.

= I'm getting the 'The plugin does not have a valid header' error =

Check the [screenshots](http://wordpress.org/extend/plugins/mage-enabler/screenshots/) area on how to setup the folder structure properly

= I'm getting a 'Decoding failed: Syntax error' or 'An error occurred while saving the attribute set' =

You might wanna take a look at what [John LeBlanc](http://wordpress.org/support/profile/johnleblanc) did [here](http://wordpress.org/support/topic/plugin-mage-enabler-workaround-for-inability-to-edit-magento-products-and-attribute-sets?replies=3).
Thanks John :)

= My question isn't answered here =

Please post it to the [WordPress support forum](http://wordpress.org/tags/mage-enabler/) and tag your post with "mage-enabler".

== Screenshots ==

1. Folder structure in installing the plugin
2. Mage Enabler running in WordPress version 2.9.2
3. Mage Enabler running in WordPress version 3.0-beta1
4. Mage Enabler running in WordPress version 3.0-beta2
5. Mage Enabler running in WordPress version 3.0 'Thelonious'

== Upgrade Notice ==

= 1.2.2 =
This fixes the bug which produces a fatal error or blank screen (for those with <code>error_reporting</code> turned off) in <code>view.php</code> for css and image url.

== Changelog ==

= 1.2.2 =
* Fixed: Fatal error or blank screen (for those with error_reporting turned off) when determining the plugin url in <code>view.php</code> file.

= 1.2.1 =
* Fixed: Sample and Related Posts not appearing due to <code>simplexml_load_file()</code> not allowed by default server configuration.

= 1.2 =
* Changed: Moved the Mage Enabler tab to <code>Settings -> Mage Enabler</code>
* Fixed: Fatal error when no 'default' store code is configured [see comment #659](http://mysillypointofview.richardferaro.com/2010/05/11/mage-enabler/#comment-659)
* Updated: Page layout and added reference for examples and related posts
* Added: Auto-detect store configuration

= 1.1.1 =
* Fixed fatal error during xmlrpc calls [see comment #417](http://mysillypointofview.richardferaro.com/2010/05/11/mage-enabler/#comment-417). Thanks [mik](http://lexiamatic.wordpress.com) for the feedback.

= 1.1 =
* Fixed fatal error when URL value provided is a directory
* Added WordPress 3.0 screenshot-5.png sample output

= 1.0 =
* Initial commit for Mage Enabler plugin