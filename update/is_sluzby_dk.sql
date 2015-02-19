-- --------------------------------------------------------
-- Hostiteľ:                     127.0.0.1
-- Verze serveru:                5.1.73-community - MySQL Community Server (GPL)
-- OS serveru:                   Win64
-- HeidiSQL Verzia:              9.1.0.4867
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Exportování struktury pro tabulka kdch_sk.is_sluzby_dk
CREATE TABLE IF NOT EXISTS `is_sluzby_dk` (
  `item_id` bigint(20) unsigned NOT NULL,
  `datum` date NOT NULL,
  `typ` varchar(10) CHARACTER SET ascii NOT NULL,
  `user_id` bigint(20) unsigned NOT NULL,
  `date_group` int(6) unsigned zerofill NOT NULL,
  `ordering` tinyint(2) NOT NULL,
  `comment` text COLLATE utf8_slovak_ci,
  `state` enum('draft','active') CHARACTER SET ascii NOT NULL DEFAULT 'draft',
  PRIMARY KEY (`item_id`),
  UNIQUE KEY `datum_typ_user_id` (`datum`,`typ`,`user_id`),
  KEY `datum` (`datum`),
  KEY `typ` (`typ`),
  KEY `user_id` (`user_id`),
  KEY `date_group` (`date_group`),
  KEY `ordering` (`ordering`),
  KEY `state` (`state`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_slovak_ci ROW_FORMAT=COMPACT;

-- Export dat nebyl vybrán.
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
