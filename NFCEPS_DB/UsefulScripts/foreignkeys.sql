-- ============================================================
-- Foreign Keys
-- ============================================================

-- User
ALTER TABLE "user".tblusers
  ADD CONSTRAINT fk_users_roleid
  FOREIGN KEY (roleid) REFERENCES permission.tblroles (roleid)
  DEFERRABLE INITIALLY IMMEDIATE;

-- Permission
ALTER TABLE permission.tblroles
  ADD CONSTRAINT fk_roles_rolepermission
  FOREIGN KEY (roleid) REFERENCES permission.tblrolepermission (roleid)
  DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE permission.tblpermission
  ADD CONSTRAINT fk_permission_rolepermission
  FOREIGN KEY (permid) REFERENCES permission.tblrolepermission (permid)
  DEFERRABLE INITIALLY IMMEDIATE;

-- Entity
ALTER TABLE entity.tblentity
  ADD CONSTRAINT fk_entity_owner
  FOREIGN KEY (ownerid) REFERENCES entity.tblentityowner (ownerid)
  DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE entity.tblentity
  ADD CONSTRAINT fk_entity_settlement
  FOREIGN KEY (entityid) REFERENCES branch.tblownersettlement (entityid)
  DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE entity.tblentity
  ADD CONSTRAINT fk_entity_route
  FOREIGN KEY (entityid) REFERENCES route.tblroute (entityid)
  DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE entity.tblentityowner
  ADD CONSTRAINT fk_entityowner_settlement
  FOREIGN KEY (ownerid) REFERENCES branch.tblownersettlement (ownerid)
  DEFERRABLE INITIALLY IMMEDIATE;

-- Machine
ALTER TABLE machine.tblmachine
  ADD CONSTRAINT fk_machine_entity
  FOREIGN KEY (entityid) REFERENCES entity.tblentity (entityid)
  DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE machine.tblmachine
  ADD CONSTRAINT fk_machine_paymenthistory
  FOREIGN KEY (machineid) REFERENCES "transaction".tbluserpaymenthistory (machineid)
  DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE machine.tblmachine
  ADD CONSTRAINT fk_machine_pendingsync
  FOREIGN KEY (machineid) REFERENCES "transaction".tblpendingsync (machineid)
  DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE machine.tblmachine
  ADD CONSTRAINT fk_machine_bussession
  FOREIGN KEY (machineid) REFERENCES machine.tblbussession (machineid)
  DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE machine.tblbussession
  ADD CONSTRAINT fk_bussession_route
  FOREIGN KEY (routeid) REFERENCES route.tblroute (routeid)
  DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE machine.tblbussession
  ADD CONSTRAINT fk_bussession_driver
  FOREIGN KEY (driverid) REFERENCES "user".tblusers (userid)
  DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE machine.tblbussession
  ADD CONSTRAINT fk_bussession_currentstop
  FOREIGN KEY (currentstopid) REFERENCES route.tblstop (stopid)
  DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE machine.tblbussession
  ADD CONSTRAINT fk_bussession_paymenthistory
  FOREIGN KEY (sessionid) REFERENCES "transaction".tbluserpaymenthistory (sessionid)
  DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE machine.tblbussession
  ADD CONSTRAINT fk_bussession_pendingsync
  FOREIGN KEY (sessionid) REFERENCES "transaction".tblpendingsync (sessionid)
  DEFERRABLE INITIALLY IMMEDIATE;

-- Route
ALTER TABLE route.tblroute
  ADD CONSTRAINT fk_route_routestop
  FOREIGN KEY (routeid) REFERENCES route.tblroutestop (routeid)
  DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE route.tblroute
  ADD CONSTRAINT fk_route_farerule
  FOREIGN KEY (routeid) REFERENCES route.tblfarerule (routeid)
  DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE route.tblstop
  ADD CONSTRAINT fk_stop_routestop
  FOREIGN KEY (stopid) REFERENCES route.tblroutestop (stopid)
  DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE route.tblstop
  ADD CONSTRAINT fk_stop_farerule_from
  FOREIGN KEY (stopid) REFERENCES route.tblfarerule (fromstopid)
  DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE route.tblstop
  ADD CONSTRAINT fk_stop_farerule_to
  FOREIGN KEY (stopid) REFERENCES route.tblfarerule (tostopid)
  DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE route.tblstop
  ADD CONSTRAINT fk_stop_paymenthistory_checkin
  FOREIGN KEY (stopid) REFERENCES "transaction".tbluserpaymenthistory (checkinstopid)
  DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE route.tblstop
  ADD CONSTRAINT fk_stop_paymenthistory_checkout
  FOREIGN KEY (stopid) REFERENCES "transaction".tbluserpaymenthistory (checkoutstopid)
  DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE route.tblstop
  ADD CONSTRAINT fk_stop_bussession_currentstop
  FOREIGN KEY (stopid) REFERENCES machine.tblbussession (currentstopid)
  DEFERRABLE INITIALLY IMMEDIATE;

-- Transaction
ALTER TABLE "transaction".tbluserpaymenthistory
  ADD CONSTRAINT fk_paymenthistory_user
  FOREIGN KEY (userid) REFERENCES "user".tblusers (userid)
  DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE "transaction".tbluserpaymenthistory
  ADD CONSTRAINT fk_paymenthistory_cardhistory
  FOREIGN KEY (payid) REFERENCES card.tblcardhistory (payid)
  DEFERRABLE INITIALLY IMMEDIATE;

-- Card
ALTER TABLE card.tblcard
  ADD CONSTRAINT fk_card_user
  FOREIGN KEY (userid) REFERENCES "user".tblusers (userid)
  DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE card.tblcard
  ADD CONSTRAINT fk_card_currentsession
  FOREIGN KEY (currentsessionid) REFERENCES machine.tblbussession (sessionid)
  DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE card.tblcard
  ADD CONSTRAINT fk_card_cardrecharge
  FOREIGN KEY (cardid) REFERENCES branch.tblcardrecharge (cardid)
  DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE card.tblcardhistory
  ADD CONSTRAINT fk_cardhistory_card
  FOREIGN KEY (cardid) REFERENCES card.tblcard (cardid)
  DEFERRABLE INITIALLY IMMEDIATE;

-- Branch
ALTER TABLE branch.tblcardrecharge
  ADD CONSTRAINT fk_cardrecharge_branch
  FOREIGN KEY (branchid) REFERENCES branch.tblbranch (branchid)
  DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE branch.tblcardrecharge
  ADD CONSTRAINT fk_cardrecharge_user
  FOREIGN KEY (userid) REFERENCES "user".tblusers (userid)
  DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE branch.tblcardrecharge
  ADD CONSTRAINT fk_cardrecharge_rechargedby
  FOREIGN KEY (rechargedby) REFERENCES "user".tblusers (userid)
  DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE branch.tblownersettlement
  ADD CONSTRAINT fk_settlement_branch
  FOREIGN KEY (branchid) REFERENCES branch.tblbranch (branchid)
  DEFERRABLE INITIALLY IMMEDIATE;

ALTER TABLE branch.tblownersettlement
  ADD CONSTRAINT fk_settlement_settledby
  FOREIGN KEY (settledby) REFERENCES "user".tblusers (userid)
  DEFERRABLE INITIALLY IMMEDIATE;