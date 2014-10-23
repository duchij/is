-- phpMyAdmin SQL Dump
-- version 2.10.1
-- http://www.phpmyadmin.net
-- 
-- Hostiteľ: localhost
-- Vygenerované:: 14.Nov, 2010 - 20:57
-- Verzia serveru: 5.0.83
-- Verzia PHP: 5.2.8

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";

-- 
-- Databáza: `kdch_sk`
-- 
CREATE DATABASE `kdch_sk` DEFAULT CHARACTER SET utf8 COLLATE utf8_slovak_ci;
USE `kdch_sk`;

-- --------------------------------------------------------

-- 
-- Štruktúra tabuľky pre tabuľku `is_hlasko`
-- 

CREATE TABLE `is_hlasko` (
  `id` bigint(20) NOT NULL auto_increment,
  `type` tinytext collate utf8_slovak_ci,
  `from` varchar(22) collate utf8_slovak_ci NOT NULL,
  `to` varchar(22) collate utf8_slovak_ci NOT NULL,
  `text` longtext collate utf8_slovak_ci,
  `date` text collate utf8_slovak_ci NOT NULL,
  `creat_user` int(11) NOT NULL,
  `last_user` int(11) NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8 COLLATE=utf8_slovak_ci AUTO_INCREMENT=5 ;

-- 
-- Sťahujem dáta pre tabuľku `is_hlasko`
-- 


-- --------------------------------------------------------

-- 
-- Štruktúra tabuľky pre tabuľku `is_users`
-- 

CREATE TABLE `is_users` (
  `id` int(11) NOT NULL auto_increment,
  `full_name` text collate utf8_slovak_ci,
  `name` text collate utf8_slovak_ci NOT NULL,
  `passwd` text collate utf8_slovak_ci NOT NULL,
  PRIMARY KEY  (`id`),
  FULLTEXT KEY `name` (`name`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8 COLLATE=utf8_slovak_ci AUTO_INCREMENT=2 ;

-- 
-- Sťahujem dáta pre tabuľku `is_users`
-- 

INSERT INTO `is_users` (`id`, `full_name`, `name`, `passwd`) VALUES 
(1, NULL, 'admin', '/abvn2uoOCyHVGjNcNM+zw==');

-- --------------------------------------------------------

-- 
-- Štruktúra tabuľky pre tabuľku `jasna2011`
-- 

CREATE TABLE `jasna2011` (
  `id` int(11) NOT NULL auto_increment,
  `titel1` text collate utf8_slovak_ci,
  `name` text collate utf8_slovak_ci NOT NULL,
  `surname` text collate utf8_slovak_ci NOT NULL,
  `titel2` text collate utf8_slovak_ci,
  `email` text collate utf8_slovak_ci NOT NULL,
  `adresa_prac` longtext collate utf8_slovak_ci NOT NULL,
  `active_status` tinytext collate utf8_slovak_ci NOT NULL,
  `active_type` tinytext collate utf8_slovak_ci NOT NULL,
  `nazov_pred` longtext collate utf8_slovak_ci,
  `autory_pred` longtext collate utf8_slovak_ci,
  `praco_pred` longtext collate utf8_slovak_ci,
  `sumar_sk` longtext collate utf8_slovak_ci,
  `sumar_en` longtext collate utf8_slovak_ci,
  `abstrakt` longtext collate utf8_slovak_ci,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8 COLLATE=utf8_slovak_ci AUTO_INCREMENT=3 ;

-- 
-- Sťahujem dáta pre tabuľku `jasna2011`
-- 

INSERT INTO `jasna2011` (`id`, `titel1`, `name`, `surname`, `titel2`, `email`, `adresa_prac`, `active_status`, `active_type`, `nazov_pred`, `autory_pred`, `praco_pred`, `sumar_sk`, `sumar_en`, `abstrakt`) VALUES 
(2, 'MUDr.', 'Boris', 'Duchaj', 'PhD.,', 'dkjfhd', 'dsfds', '1', 'S', 'dfsdfds', 'sdfdsf', 'dsfsdf', 'sdfdf', 'sdfsdf', 'dfsdfsd');

-- --------------------------------------------------------

-- 
-- Štruktúra tabuľky pre tabuľku `trauma2010`
-- 

CREATE TABLE `trauma2010` (
  `id` int(11) NOT NULL auto_increment,
  `titul1` text collate utf8_slovak_ci,
  `meno` text collate utf8_slovak_ci,
  `titul2` text collate utf8_slovak_ci,
  `adr_prac` text collate utf8_slovak_ci NOT NULL,
  `adr_bydl` text collate utf8_slovak_ci NOT NULL,
  `aktiv_ucast` text collate utf8_slovak_ci NOT NULL,
  `email` text collate utf8_slovak_ci NOT NULL,
  `nazov_pred` longtext collate utf8_slovak_ci NOT NULL,
  `aut_pred` longtext collate utf8_slovak_ci NOT NULL,
  `abstrakt` longtext collate utf8_slovak_ci NOT NULL,
  PRIMARY KEY  (`id`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8 COLLATE=utf8_slovak_ci AUTO_INCREMENT=4 ;

-- 
-- Sťahujem dáta pre tabuľku `trauma2010`
-- 

INSERT INTO `trauma2010` (`id`, `titul1`, `meno`, `titul2`, `adr_prac`, `adr_bydl`, `aktiv_ucast`, `email`, `nazov_pred`, `aut_pred`, `abstrakt`) VALUES 
(1, 'fsdfdf', 'dfsdfds', 'sdfsdf', 'sdfdsf', 'sdfdsf', 'Ano', 'bduchaj@gmail.com', 'ffsdf', 'dsfdsf', 'dsfdfs'),
(2, 'dsfdf', 'sdfdfd', 'dsfdsfdf', 'dfdsf', 'dsfdsf', 'Ano', 'bduchaj@gmail.com', 'dfdfdsf', 'sdfdsfd', 'sdfsdfdf'),
(3, 'dfdf', 'dfdf', 'dfdf', 'dfdf', 'dfdf', 'Ano', 'bduchaj@gmail.com', 'fdfdf', 'dfdf', 'dfdf');
