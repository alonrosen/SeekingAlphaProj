CREATE DATABASE `seekingalpha` /*!40100 DEFAULT CHARACTER SET utf8 */;

CREATE TABLE `groups` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

CREATE TABLE `users` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(45) DEFAULT NULL,
  `group_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_group_id_idx` (`group_id`),
  CONSTRAINT `fk_group_id` FOREIGN KEY (`group_id`) REFERENCES `groups` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8;

CREATE TABLE `user_follow` (
  `user_id` int(11) NOT NULL,
  `followed_user_id` int(11) NOT NULL,
  PRIMARY KEY (`user_id`,`followed_user_id`),
  KEY `fk_user_id_2_idx` (`followed_user_id`),
  CONSTRAINT `fk_user_id_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_user_id_2` FOREIGN KEY (`followed_user_id`) REFERENCES `users` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `follow_unfollow`(in act varchar(1), in  following_uid int, in followed_uid int)
BEGIN
	if act = 'F' then
		insert into user_follow(user_id, followed_user_id) values (following_uid, followed_uid);
	elseif act = 'U' then
		delete from user_follow where user_id = following_uid and followed_user_id = followed_uid;
    end if;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `get_user_following`(in i_user_id int)
BEGIN
	select u.id, u.name, g.name as group_name, 
		(select count(1) from user_follow uf where uf.followed_user_id = u.id) as followers,
        case 
			when (select count(1) from user_follow uf where  uf.followed_user_id = u.id and uf.user_id = i_user_id) > 0 then
            true
        else
			false
		end as following
        
	from users u
	inner join groups as g on g.id = u.group_id
	where u.id != i_user_id
    and exists (select 1 from users where id = i_user_id);

END$$
DELIMITER ;
