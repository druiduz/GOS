-- phpMyAdmin SQL Dump
-- version 3.4.10.1
-- http://www.phpmyadmin.net
--
-- Client: localhost
-- Généré le : Ven 03 Mai 2013 à 10:16
-- Version du serveur: 5.5.20
-- Version de PHP: 5.3.10

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Base de données: `gos`
--

-- --------------------------------------------------------

--
-- Structure de la table `client`
--

DROP TABLE IF EXISTS `client`;
CREATE TABLE IF NOT EXISTS `client` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `nom` varchar(45) NOT NULL,
  `prenom` varchar(45) NOT NULL,
  `solde` float NOT NULL,
  `rfid_ID` varchar(10) NOT NULL DEFAULT '',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=6 ;

--
-- Contenu de la table `client`
--

INSERT INTO `client` (`id`, `nom`, `prenom`, `solde`, `rfid_ID`) VALUES
(0, 'anonymous', '', 0, ''),
(1, 'client1_n', 'client1_p', 0.8, ''),
(3, 'Allier', 'Mikael', 50, '469627FE'),
(4, 'Lavergne', 'Guillaume', 25, 'E20F9BEB'),
(5, 'test', 'cart', 50, '');

-- --------------------------------------------------------

--
-- Structure de la table `produit`
--

DROP TABLE IF EXISTS `produit`;
CREATE TABLE IF NOT EXISTS `produit` (
  `idProduit` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `Nom_Produit` varchar(45) NOT NULL,
  `Type_Produit` varchar(45) NOT NULL,
  `Prix_Produit` float NOT NULL,
  `Quantite` int(10) unsigned NOT NULL,
  `Quantite_mini` int(10) unsigned NOT NULL,
  `Logo` varchar(200) NOT NULL,
  PRIMARY KEY (`idProduit`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=11 ;

--
-- Contenu de la table `produit`
--

INSERT INTO `produit` (`idProduit`, `Nom_Produit`, `Type_Produit`, `Prix_Produit`, `Quantite`, `Quantite_mini`, `Logo`) VALUES
(1, 'coca', 'boisson', 0.6, 37, 5, 'coca-cola-logo.jpg'),
(2, 'cafe', 'boisson', 0.4, 41, 5, 'cafe.jpg'),
(3, 'm&m', 'nourriture', 0.6, 37, 5, 'mms-logo.jpg'),
(4, 'lion', 'nourriture', 0.6, 50, 5, 'lion.jpg'),
(5, 'redbull', 'autre', 1, 50, 5, 'redbull.jpg'),
(6, 'bounty', 'nourriture', 0, 0, 5, 'bounty.jpg'),
(7, 'fanta', 'boisson', 0.6, 50, 5, 'fanta.png'),
(8, 'm&m noir', 'nourriture', 0.6, 50, 5, 'mms-noir-logo.png'),
(9, 'sucettes', 'nourriture', 0.2, 200, 5, 'sucettes.jpg'),
(10, 'kit kat', 'nourriture', 0.6, 50, 5, 'Kit-Kat.jpg');

-- --------------------------------------------------------

--
-- Structure de la table `vendeur`
--

DROP TABLE IF EXISTS `vendeur`;
CREATE TABLE IF NOT EXISTS `vendeur` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `login` varchar(45) NOT NULL,
  `mdp` varchar(45) NOT NULL,
  `nom` varchar(45) NOT NULL,
  `prenom` varchar(45) NOT NULL,
  `lastCo` datetime NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=3 ;

--
-- Contenu de la table `vendeur`
--

INSERT INTO `vendeur` (`id`, `login`, `mdp`, `nom`, `prenom`, `lastCo`) VALUES
(1, 'admin', '21232f297a57a5a743894a0e4a801fc3', 'vendeur', '1', '2013-05-03 11:56:51'),
(2, 'druiduz', '63a9f0ea7bb98050796b649e85481845', 'Allier', 'Mikael', '0000-00-00 00:00:00');

-- --------------------------------------------------------

--
-- Structure de la table `vente`
--

DROP TABLE IF EXISTS `vente`;
CREATE TABLE IF NOT EXISTS `vente` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `client_id` int(10) unsigned NOT NULL,
  `vendeur_id` int(10) unsigned NOT NULL,
  `date_vente` datetime NOT NULL,
  `total` float NOT NULL DEFAULT '0',
  `type` varchar(10) NOT NULL,
  `rendu` float NOT NULL,
  PRIMARY KEY (`id`),
  KEY `FK_Vendeur` (`vendeur_id`),
  KEY `FK_client` (`client_id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=17 ;

--
-- Contenu de la table `vente`
--

INSERT INTO `vente` (`id`, `client_id`, `vendeur_id`, `date_vente`, `total`, `type`, `rendu`) VALUES
(1, 1, 1, '2013-04-18 22:51:41', 0, '', 0),
(2, 1, 1, '2013-04-18 23:05:43', 0, '', 0),
(3, 1, 1, '2013-04-18 23:07:44', 0, '', 0),
(4, 1, 1, '2013-04-18 23:11:43', 0, '', 0),
(5, 1, 1, '2013-04-18 23:12:19', 0, '', 0),
(6, 1, 1, '2013-04-18 23:13:36', 0, '', 0),
(7, 1, 1, '2013-04-18 23:14:45', 0, '', 0),
(8, 1, 1, '2013-04-18 23:15:35', 0, '', 0),
(9, 1, 1, '2013-04-19 11:31:46', 0, '', 0),
(10, 0, 1, '2013-04-28 16:53:04', 0, '', 0),
(11, 0, 1, '2013-04-28 16:55:30', 1.8, '', 0),
(12, 1, 1, '2013-04-29 12:12:55', 3.6, 'carte', 0),
(13, 1, 1, '2013-04-30 16:01:02', 1.2, 'carte', 0),
(14, 1, 1, '2013-05-02 01:32:21', 2.4, 'carte', 0),
(15, 0, 1, '2013-05-02 01:33:10', 6, 'espece', 94),
(16, 0, 1, '2013-05-02 09:10:01', 1.8, 'espece', 109.2);

-- --------------------------------------------------------

--
-- Structure de la table `ventedetails`
--

DROP TABLE IF EXISTS `ventedetails`;
CREATE TABLE IF NOT EXISTS `ventedetails` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `vente_id` int(10) unsigned NOT NULL,
  `produit_id` int(10) unsigned NOT NULL,
  `quantite` int(10) unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `FK_ventedetails_vente_id` (`vente_id`),
  KEY `FK_ventedetails_produit_id` (`produit_id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=29 ;

--
-- Contenu de la table `ventedetails`
--

INSERT INTO `ventedetails` (`id`, `vente_id`, `produit_id`, `quantite`) VALUES
(1, 5, 5, 1),
(2, 5, 2, 2),
(3, 5, 1, 1),
(4, 5, 4, 1),
(5, 5, 3, 1),
(6, 6, 1, 1),
(7, 6, 2, 1),
(8, 6, 5, 1),
(9, 6, 4, 1),
(10, 7, 1, 2),
(11, 7, 2, 2),
(12, 7, 5, 1),
(13, 8, 1, 2),
(14, 8, 2, 1),
(15, 9, 1, 1),
(16, 9, 2, 1),
(17, 10, 2, 1),
(18, 10, 1, 2),
(19, 11, 2, 3),
(20, 12, 2, 3),
(21, 12, 1, 1),
(22, 12, 1, 2),
(23, 13, 1, 2),
(24, 14, 1, 2),
(25, 14, 3, 2),
(26, 15, 3, 10),
(27, 16, 1, 2),
(28, 16, 3, 1);

--
-- Contraintes pour les tables exportées
--

--
-- Contraintes pour la table `vente`
--
ALTER TABLE `vente`
  ADD CONSTRAINT `FK_client` FOREIGN KEY (`client_id`) REFERENCES `client` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `FK_Vendeur` FOREIGN KEY (`vendeur_id`) REFERENCES `vendeur` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Contraintes pour la table `ventedetails`
--
ALTER TABLE `ventedetails`
  ADD CONSTRAINT `FK_ventedetails_produit_id` FOREIGN KEY (`produit_id`) REFERENCES `produit` (`idProduit`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `FK_ventedetails_vente_id` FOREIGN KEY (`vente_id`) REFERENCES `vente` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
