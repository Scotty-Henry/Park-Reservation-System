Select * From reservation Where campground_id = 1 AND from_date Between '2019/06/17' AND

Select * From site;

Select * 
from reservation
join UserReservation ON reservation.reservation_id = UserReservation.reservation_id
where [user_id] = 1;

Select *
From site
Where campground_id = 1 AND site_id Not IN (Select site.site_id
From site
Join reservation On reservation.site_id = site.site_id
Where campground_id = 1 AND to_date > '2019-06-17' 
OR from_date < '2019-06-22');

Select *
from site
where site_id NOT IN
(Select site.site_id
From site
Join reservation On reservation.site_id = site.site_id
Where (from_date <= '2019-06-17' AND to_date >= '2019-06-17')
OR (from_date < '2019-06-22' AND to_date >= '2019-06-22')
OR ('2019-06-17' <= from_date AND '2019-06-22' >= from_date));

select *
from reservation
where from_date BETWEEN '2019-06-17' AND '2019-07-17';

select *
from 

