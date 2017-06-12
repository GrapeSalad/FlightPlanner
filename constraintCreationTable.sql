USE flight_planner;
CREATE TABLE cities( id INT IDENTITY(1,1), name VARCHAR(50));
CREATE TABLE airlines (id INT IDENTITY(1,1), name VARCHAR(50), fare INT);
CREATE TABLE flights (id INT IDENTITY(1,1), arrival VARCHAR(50), departure VARCHAR(50), status VARCHAR(50), CONSTRAINT chk_Status CHECK(Status IN ('On-Time', 'Delayed', 'Cancelled', 'Complete')), airline_id INT, city_id INT);
SELECT * FROM flights;