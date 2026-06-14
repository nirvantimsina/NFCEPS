-- ============================================================
-- PostgreSQL Schema
-- Converted from MSSQL/DBML-style DDL
-- ============================================================

-- Schemas
CREATE SCHEMA IF NOT EXISTS "user";
CREATE SCHEMA IF NOT EXISTS permission;
CREATE SCHEMA IF NOT EXISTS "transaction";
CREATE SCHEMA IF NOT EXISTS entity;
CREATE SCHEMA IF NOT EXISTS machine;
CREATE SCHEMA IF NOT EXISTS card;
CREATE SCHEMA IF NOT EXISTS branch;
CREATE SCHEMA IF NOT EXISTS route;


-- ============================================================
-- User Schema
-- ============================================================

CREATE TABLE "user".tblusers (
  userid    SERIAL PRIMARY KEY,
  rfid      VARCHAR(20),
  username  VARCHAR(100),
  roleid    INT,
  name      VARCHAR(100),
  address   VARCHAR(100),
  phone     VARCHAR(20),
  isactive  BOOLEAN,
  password  BYTEA,
  createdat TIMESTAMP
);

CREATE UNIQUE INDEX ix_tblusers_rfid ON "user".tblusers (rfid);


-- ============================================================
-- Permission Schema
-- ============================================================

CREATE TABLE permission.tblroles (
  roleid   SERIAL PRIMARY KEY,
  rolename VARCHAR(50)
);

CREATE TABLE permission.tblpermission (
  permid  INT PRIMARY KEY,
  permkey VARCHAR(20),
  label   VARCHAR(20)
);

CREATE TABLE permission.tblrolepermission (
  roleid    INT,
  permid    INT,
  isallowed BOOLEAN,
  PRIMARY KEY (roleid, permid)
);


-- ============================================================
-- Entity Schema
-- ============================================================

CREATE TABLE entity.tblentityowner (
  ownerid    SERIAL PRIMARY KEY,
  address_1  VARCHAR(200),
  address_2  VARCHAR(200),
  name       VARCHAR(100),
  phone      VARCHAR(100),
  createdat  TIMESTAMP,
  isactive   BOOLEAN
);

CREATE TABLE entity.tblentity (
  entityid       SERIAL PRIMARY KEY,
  entityname     VARCHAR(200),
  ownerid        INT,
  createdat      TIMESTAMP,
  entitylocation VARCHAR(200),
  isactive       BOOLEAN
);


-- ============================================================
-- Route Schema
-- ============================================================

CREATE TABLE route.tblstop (
  stopid   SERIAL PRIMARY KEY,
  stopname VARCHAR(100),
  isactive BOOLEAN
);

CREATE TABLE route.tblroute (
  routeid   SERIAL PRIMARY KEY,
  routename VARCHAR(100),
  entityid  INT,
  isactive  BOOLEAN
);

CREATE TABLE route.tblroutestop (
  routestopid SERIAL PRIMARY KEY,
  routeid     INT,
  stopid      INT,
  stoporder   INT
);

CREATE TABLE route.tblfarerule (
  fareid     SERIAL PRIMARY KEY,
  routeid    INT,
  fromstopid INT,
  tostopid   INT,
  fare       NUMERIC(18,2)
);


-- ============================================================
-- Machine Schema
-- ============================================================

CREATE TABLE machine.tblmachine (
  machineid        SERIAL PRIMARY KEY,
  entityid         INT,
  apikey           BYTEA,
  apikeyexpiresat  TIMESTAMP,
  machinelocation  VARCHAR(200),
  isactive         BOOLEAN,
  lastauthat       TIMESTAMP,
  lastsyncat       TIMESTAMP
);

CREATE TABLE machine.tblbussession (
  sessionid     SERIAL PRIMARY KEY,
  machineid     INT,
  routeid       INT,
  driverid      INT,
  startedat     TIMESTAMP,
  endedat       TIMESTAMP,
  currentstopid INT,
  status        VARCHAR(20)
);


-- ============================================================
-- Transaction Schema
-- ============================================================

CREATE TABLE "transaction".tbluserpaymenthistory (
  payid          SERIAL PRIMARY KEY,
  userid         INT,
  machineid      INT,
  checkinstopid  INT,
  checkoutstopid INT,
  checkinat      TIMESTAMP,
  checkoutat     TIMESTAMP,
  fare           NUMERIC(18,2),
  sessionid      INT,
  status         VARCHAR(20)
);

CREATE TABLE "transaction".tblpendingsync (
  syncid        SERIAL PRIMARY KEY,
  machineid     INT,
  sessionid     INT,
  payload       TEXT,
  receivedat    TIMESTAMP,
  retrycount    INT,
  lasttriedat   TIMESTAMP,
  errormessage  VARCHAR(500),
  status        VARCHAR(20)
);


-- ============================================================
-- Card Schema
-- ============================================================

CREATE TABLE card.tblcard (
  cardid               SERIAL PRIMARY KEY,
  userid               INT,
  availableamount      NUMERIC(18,2),
  lasttransactionid    VARCHAR(50),
  currentsessionid     INT,
  currentcheckinstopid INT,
  checkinat            TIMESTAMP,
  isactive             BOOLEAN,
  lastuse              TIMESTAMP,
  deactivatedat        TIMESTAMP,
  sectorkey            BYTEA
);

CREATE TABLE card.tblcardhistory (
  id              SERIAL PRIMARY KEY,
  cardid          INT,
  userid          INT,
  transactionat   TIMESTAMP,
  payid           INT,
  rechargeid      INT,
  amount          NUMERIC(18,2),
  balanceafter    NUMERIC(18,2),
  transactiontype VARCHAR(20)
);


-- ============================================================
-- Branch Schema
-- ============================================================

CREATE TABLE branch.tblbranch (
  branchid  SERIAL PRIMARY KEY,
  name      VARCHAR(100),
  address   VARCHAR(200),
  phone     VARCHAR(20),
  createdat TIMESTAMP
);

CREATE TABLE branch.tblcardrecharge (
  rechargeid  SERIAL PRIMARY KEY,
  branchid    INT,
  cardid      INT,
  userid      INT,
  amount      NUMERIC(18,2),
  rechargedat TIMESTAMP,
  rechargedby INT
);

CREATE TABLE branch.tblownersettlement (
  settlementid SERIAL PRIMARY KEY,
  entityid     INT,
  ownerid      INT,
  branchid     INT,
  amount       NUMERIC(18,2),
  settledat    TIMESTAMP,
  notes        VARCHAR(500),
  settledby    INT
);