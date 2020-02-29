<?php
/*
Plugin Name: Mage Enabler
Plugin URI: http://mysillypointofview.richardferaro.com/2010/05/11/mage-enabler/
Description: A plugin which enables Magento's session to run within WordPress.
Author: Richard Feraro
Author URI: http://www.richardferaro.com
Version: 1.2.2
Stable tag: 1.2.2
License: GPL2

    Copyright 2010  Richard D. Feraro  (email : richardferaro@gmail.com)

    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License, version 2, as 
    published by the Free Software Foundation.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
*/

function mage_enabler() {
    $mage_php_url = get_option( 'mage_php_url' );
	
    if ( !empty( $mage_php_url ) && file_exists( $mage_php_url ) && !is_dir( $mage_php_url )) {
        // Include Magento's Mage.php file
        require_once ( $mage_php_url );
       	umask(0);
		
		Mage::app();
    }
}

function mage_enabler_get_feed() {
	try {
		$url = "http://mysillypointofview.richardferaro.com/feed/?s=".urlencode("Magento")."&submit=";

		$curl = curl_init();
		curl_setopt($curl, CURLOPT_URL, $url);
		curl_setopt($curl, CURLOPT_RETURNTRANSFER, true);
		$string = curl_exec($curl);
		curl_close($curl);

		$xml = simplexml_load_string($string);

		return $xml;
	}catch(Exception $e){
		return false;
	}
}

function mage_enabler_menu() {
	//create new menu in Settings -> Mage Enabler
	add_options_page( 'Mage Enabler Settings', 'Mage Enabler', 'administrator', 'mage-enabler', 'mage_enabler_page' );
	//call register settings function
	add_action( 'admin_init', 'mage_enabler_settings' );
}

function mage_enabler_page() {
	// get absolute url of Mage.php from db
    $mage_php_url = get_option( 'mage_php_url' );
	
	// notification/error messages
	if ( !empty( $mage_php_url ) && !file_exists( $mage_php_url ) ) {
		$message = 'Invalid URL';
	} elseif ( !empty( $mage_php_url ) && file_exists( $mage_php_url ) ) {
		mage_enabler();
		$message = ( class_exists( 'Mage' ) ) ? 'File is accessible!' : 'Mage object not found!';
	} else {
		$message = '';
	}

	require_once "view.php";
}

function mage_enabler_settings() {
    // required mage enabler settings
    register_setting( 'mage-enabler-settings', 'mage_php_url' );
}

/** @todo ... Allow Mage object to run in admin pages too */
is_admin() ? add_action( 'admin_menu', 'mage_enabler_menu' ) : '';
!is_admin() ? add_action( 'wp', 'mage_enabler' ) : '';
?>