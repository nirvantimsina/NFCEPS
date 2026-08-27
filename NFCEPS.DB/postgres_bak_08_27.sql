--
-- PostgreSQL database cluster dump
--

-- Started on 2026-08-27 09:54:07

\restrict etLEGsvedSl1mGGxHieG3lHTvoLSUHtVpX3deeDG0xosg1HwtFAhdzMoq6opWME

SET default_transaction_read_only = off;

SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;

--
-- Roles
--

CREATE ROLE postgres;
ALTER ROLE postgres WITH SUPERUSER INHERIT CREATEROLE CREATEDB LOGIN REPLICATION BYPASSRLS;

--
-- User Configurations
--








\unrestrict etLEGsvedSl1mGGxHieG3lHTvoLSUHtVpX3deeDG0xosg1HwtFAhdzMoq6opWME

--
-- Databases
--

--
-- Database "template1" dump
--

\connect template1

--
-- PostgreSQL database dump
--

\restrict HbEybh1HnCPC0YdyJwdgcl3Mr9qdJU1AEx5PJbMdldsIvqScmOl2gCr4zy91XCD

-- Dumped from database version 18.4
-- Dumped by pg_dump version 18.4

-- Started on 2026-08-27 09:54:07

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

-- Completed on 2026-08-27 09:54:07

--
-- PostgreSQL database dump complete
--

\unrestrict HbEybh1HnCPC0YdyJwdgcl3Mr9qdJU1AEx5PJbMdldsIvqScmOl2gCr4zy91XCD

--
-- Database "nfceps" dump
--

--
-- PostgreSQL database dump
--

\restrict sIDEQUZLFGzZW2tVlbjRZyTmgI9Pcs4knpeu82F9MbLOc2VHwhQLnkJLMyzFlc5

-- Dumped from database version 18.4
-- Dumped by pg_dump version 18.4

-- Started on 2026-08-27 09:54:07

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 5187 (class 1262 OID 24765)
-- Name: nfceps; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE nfceps WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'C';


ALTER DATABASE nfceps OWNER TO postgres;

\unrestrict sIDEQUZLFGzZW2tVlbjRZyTmgI9Pcs4knpeu82F9MbLOc2VHwhQLnkJLMyzFlc5
\connect nfceps
\restrict sIDEQUZLFGzZW2tVlbjRZyTmgI9Pcs4knpeu82F9MbLOc2VHwhQLnkJLMyzFlc5

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 12 (class 2615 OID 24772)
-- Name: branch; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA branch;


ALTER SCHEMA branch OWNER TO postgres;

--
-- TOC entry 11 (class 2615 OID 24771)
-- Name: card; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA card;


ALTER SCHEMA card OWNER TO postgres;

--
-- TOC entry 9 (class 2615 OID 24769)
-- Name: entity; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA entity;


ALTER SCHEMA entity OWNER TO postgres;

--
-- TOC entry 10 (class 2615 OID 24770)
-- Name: machine; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA machine;


ALTER SCHEMA machine OWNER TO postgres;

--
-- TOC entry 7 (class 2615 OID 24767)
-- Name: permission; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA permission;


ALTER SCHEMA permission OWNER TO postgres;

--
-- TOC entry 13 (class 2615 OID 24773)
-- Name: route; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA route;


ALTER SCHEMA route OWNER TO postgres;

--
-- TOC entry 8 (class 2615 OID 24768)
-- Name: transaction; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA transaction;


ALTER SCHEMA transaction OWNER TO postgres;

--
-- TOC entry 6 (class 2615 OID 24766)
-- Name: user; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA "user";


ALTER SCHEMA "user" OWNER TO postgres;

--
-- TOC entry 286 (class 1255 OID 41029)
-- Name: assign_card_by_userid(integer, integer, integer, text); Type: PROCEDURE; Schema: card; Owner: postgres
--

CREATE PROCEDURE card.assign_card_by_userid(IN _userid integer, INOUT cardid integer DEFAULT NULL::integer, INOUT status integer DEFAULT 0, INOUT msg text DEFAULT 'Success'::text)
    LANGUAGE plpgsql
    AS $$
DECLARE 
    t_new_cardid INT;
BEGIN
    status := 0;
    msg := 'Success';

    IF EXISTS (SELECT 1 FROM "user".tblusers WHERE tblusers.userid = _userid AND tblusers.cardid IS NOT NULL) THEN
        status := 1;
        msg := 'Card has already been assigned to this user!';
        RETURN; 

    ELSIF NOT EXISTS (SELECT 1 FROM "user".tblusers WHERE tblusers.userid = _userid) THEN
        status := 2;
        msg := 'Card assign failed, the user does not exist!';
        RETURN;

    ELSE
        -- 1. Insert step
        INSERT INTO "card".tblcard (userid)
        VALUES (_userid)
        RETURNING tblcard.cardid INTO t_new_cardid;

        -- 2. Update step
        UPDATE "user".tblusers
        SET cardid = t_new_cardid
        WHERE tblusers.userid = _userid;

        -- 3. Set your output results cleanly
        cardid := t_new_cardid;
        status := 0;
        msg := 'Card assigned successfully!';
        
        -- REMOVED COMMIT; (PostgreSQL handles it automatically)
    END IF;

EXCEPTION
    WHEN OTHERS THEN
        -- REMOVED ROLLBACK; (PostgreSQL already rolled back the block before hitting this line)
        cardid := NULL;
        status := -1;
        msg := 'A critical database error occurred: ' || SQLERRM;
END;
$$;


ALTER PROCEDURE card.assign_card_by_userid(IN _userid integer, INOUT cardid integer, INOUT status integer, INOUT msg text) OWNER TO postgres;

--
-- TOC entry 285 (class 1255 OID 32779)
-- Name: fn_auth(character varying, character varying, character varying, character varying, character varying, character varying); Type: FUNCTION; Schema: permission; Owner: postgres
--

CREATE FUNCTION permission.fn_auth(p_flag character varying, p_username character varying, p_name character varying DEFAULT NULL::character varying, p_address character varying DEFAULT NULL::character varying, p_phone character varying DEFAULT NULL::character varying, p_password character varying DEFAULT NULL::character varying) RETURNS TABLE(userid integer, username character varying, name character varying, password character varying, isactive boolean, roleid integer, rolename character varying, compressedpermissions text)
    LANGUAGE plpgsql
    AS $$
BEGIN
    -- SignUp Flag: Perform Insert and return nothing
    IF p_flag = 'A' THEN
        INSERT INTO "user".tblusers (            
            username, roleid, name, address, phone, isactive, password, createdat
        )
        VALUES (
            p_username, 2, p_name, p_address, p_phone, TRUE, p_password::bytea, CURRENT_TIMESTAMP
        );

    -- Login Flag: Stream matching user rows out along with their permissions aggregator
    ELSIF p_flag = 'B' THEN
        RETURN QUERY
        SELECT 
            u.userid::integer, 
            u.username::character varying,
            u.name::character varying,
            TRIM(convert_from(u.password, 'UTF8'))::character varying AS password, 
            u.isactive::boolean,
            u.roleid::integer,
            r.rolename::character varying,
            COALESCE(string_agg(p.permkey, ','), '')::text AS compressedpermissions
        FROM "user".tblusers u
        INNER JOIN permission.tblroles r ON r.roleid = u.roleid
        LEFT JOIN permission.tblrolepermission rp ON rp.roleid = r.roleid
        LEFT JOIN permission.tblpermission p ON rp.permid = p.permid
        WHERE u.username = p_username
        -- FIXED: Explicitly grouped r.rolename matching its exact SELECT expression layout
        GROUP BY u.userid, u.username, u.name, u.password, u.isactive, u.roleid, r.rolename;
    END IF;
END;
$$;


ALTER FUNCTION permission.fn_auth(p_flag character varying, p_username character varying, p_name character varying, p_address character varying, p_phone character varying, p_password character varying) OWNER TO postgres;

--
-- TOC entry 282 (class 1255 OID 41017)
-- Name: fn_menulist(character, integer); Type: FUNCTION; Schema: permission; Owner: postgres
--

CREATE FUNCTION permission.fn_menulist(p_flag character, p_roleid integer) RETURNS TABLE(menuid integer, menuname character varying, parentid integer, icon character varying, path character varying, menuorder integer)
    LANGUAGE plpgsql
    AS $$
begin
	if p_flag = 'A' then
		return query
		select ml.menuid, ml.menuname, ml.parentid, ml.icon, ml.path, ml.menuorder from tblmenulist ml
		left join permission.tblmenurole mr on mr.menuid = ml.menuid
		where mr.roleid  = p_roleid;
	end if;
END;
$$;


ALTER FUNCTION permission.fn_menulist(p_flag character, p_roleid integer) OWNER TO postgres;

--
-- TOC entry 269 (class 1255 OID 24946)
-- Name: sp_getallrolepermissions(refcursor); Type: PROCEDURE; Schema: permission; Owner: postgres
--

CREATE PROCEDURE permission.sp_getallrolepermissions(INOUT p_refcursor refcursor DEFAULT 'rs_result'::refcursor)
    LANGUAGE plpgsql
    AS $$
BEGIN
OPEN p_refcursor FOR 
    SELECT 
        rp.roleid, 
        p.permid 
    FROM permission.tblpermission p
    LEFT JOIN permission.tblrolepermission rp ON rp.permid = p.permid;
END;
$$;


ALTER PROCEDURE permission.sp_getallrolepermissions(INOUT p_refcursor refcursor) OWNER TO postgres;

--
-- TOC entry 283 (class 1255 OID 32776)
-- Name: fn_dashboard(character varying, integer); Type: FUNCTION; Schema: public; Owner: postgres
--

CREATE FUNCTION public.fn_dashboard(p_flag character varying DEFAULT NULL::character varying, p_userid integer DEFAULT NULL::integer) RETURNS TABLE(name character varying, userrole character varying, address character varying, phone character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN
    IF p_flag = 'A' THEN
        RETURN QUERY
        SELECT 
            u.name::character varying, 
            r.rolename::character varying AS userrole, 
            u.address::character varying, 
            u.phone::character varying
        FROM "user".tblusers u
        LEFT JOIN permission.tblroles r ON r.roleid = u.roleid
        WHERE u.userid = p_userid;
    END IF;
END;
$$;


ALTER FUNCTION public.fn_dashboard(p_flag character varying, p_userid integer) OWNER TO postgres;

--
-- TOC entry 281 (class 1255 OID 41025)
-- Name: insert_menulist(character varying, integer, character varying, character varying, integer, integer, integer, text); Type: PROCEDURE; Schema: public; Owner: postgres
--

CREATE PROCEDURE public.insert_menulist(IN p_menuname character varying, IN p_parentid integer, IN p_icon character varying, IN p_path character varying, IN p_menuorder integer, IN p_createdby integer, INOUT p_status integer DEFAULT 0, INOUT p_msg text DEFAULT 'Success'::text)
    LANGUAGE plpgsql
    AS $$
declare v_rows_affected INT;
begin
	insert into tblmenulist(menuname, parentid, icon, "path", menuorder, createdby)
	values (p_menuname, p_parentid, p_icon, p_path, p_menuorder, p_createdby);

	GET DIAGNOSTICS v_rows_affected = ROW_COUNT;

	if v_rows_affected > 0 THEN
		p_status := 0;
		p_msg := 'Menu item inserted successfully!';

		commit;
	ELSE
		p_status := 1;
		p_msg := 'Could not insert menu item!';
	end if;
exception
	when others THEN
	rollback;

	p_status := 1;
	p_msg := 'Database error occured!';
end;
$$;


ALTER PROCEDURE public.insert_menulist(IN p_menuname character varying, IN p_parentid integer, IN p_icon character varying, IN p_path character varying, IN p_menuorder integer, IN p_createdby integer, INOUT p_status integer, INOUT p_msg text) OWNER TO postgres;

--
-- TOC entry 284 (class 1255 OID 41023)
-- Name: fn_userreport(character, integer); Type: FUNCTION; Schema: user; Owner: postgres
--

CREATE FUNCTION "user".fn_userreport(p_flag character, p_userid integer) RETURNS TABLE(userid integer, username character varying, roleid integer, name character varying, address character varying, phone character varying, cardid integer)
    LANGUAGE plpgsql
    AS $$
BEGIN
    IF p_flag = 'A' THEN
        RETURN QUERY
        SELECT 
            u.userid, 
            u.username, 
            u.roleid, 
            u.name, 
            u.address, 
            u.phone, 
            u.cardid 
        FROM "user".tblusers u
        WHERE u.isactive = true 
          AND u.userid = COALESCE(p_userid, u.userid);
    END IF;
END;
$$;


ALTER FUNCTION "user".fn_userreport(p_flag character, p_userid integer) OWNER TO postgres;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 256 (class 1259 OID 24909)
-- Name: tblbranch; Type: TABLE; Schema: branch; Owner: postgres
--

CREATE TABLE branch.tblbranch (
    branchid integer NOT NULL,
    name character varying(100),
    address character varying(200),
    phone character varying(20),
    createdat timestamp without time zone
);


ALTER TABLE branch.tblbranch OWNER TO postgres;

--
-- TOC entry 255 (class 1259 OID 24908)
-- Name: tblbranch_branchid_seq; Type: SEQUENCE; Schema: branch; Owner: postgres
--

CREATE SEQUENCE branch.tblbranch_branchid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE branch.tblbranch_branchid_seq OWNER TO postgres;

--
-- TOC entry 5188 (class 0 OID 0)
-- Dependencies: 255
-- Name: tblbranch_branchid_seq; Type: SEQUENCE OWNED BY; Schema: branch; Owner: postgres
--

ALTER SEQUENCE branch.tblbranch_branchid_seq OWNED BY branch.tblbranch.branchid;


--
-- TOC entry 258 (class 1259 OID 24917)
-- Name: tblcardrecharge; Type: TABLE; Schema: branch; Owner: postgres
--

CREATE TABLE branch.tblcardrecharge (
    rechargeid integer NOT NULL,
    branchid integer,
    cardid integer,
    userid integer,
    amount numeric(18,2),
    rechargedat timestamp without time zone,
    rechargedby integer
);


ALTER TABLE branch.tblcardrecharge OWNER TO postgres;

--
-- TOC entry 257 (class 1259 OID 24916)
-- Name: tblcardrecharge_rechargeid_seq; Type: SEQUENCE; Schema: branch; Owner: postgres
--

CREATE SEQUENCE branch.tblcardrecharge_rechargeid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE branch.tblcardrecharge_rechargeid_seq OWNER TO postgres;

--
-- TOC entry 5189 (class 0 OID 0)
-- Dependencies: 257
-- Name: tblcardrecharge_rechargeid_seq; Type: SEQUENCE OWNED BY; Schema: branch; Owner: postgres
--

ALTER SEQUENCE branch.tblcardrecharge_rechargeid_seq OWNED BY branch.tblcardrecharge.rechargeid;


--
-- TOC entry 260 (class 1259 OID 24925)
-- Name: tblownersettlement; Type: TABLE; Schema: branch; Owner: postgres
--

CREATE TABLE branch.tblownersettlement (
    settlementid integer NOT NULL,
    entityid integer,
    ownerid integer,
    branchid integer,
    amount numeric(18,2),
    settledat timestamp without time zone,
    notes character varying(500),
    settledby integer
);


ALTER TABLE branch.tblownersettlement OWNER TO postgres;

--
-- TOC entry 259 (class 1259 OID 24924)
-- Name: tblownersettlement_settlementid_seq; Type: SEQUENCE; Schema: branch; Owner: postgres
--

CREATE SEQUENCE branch.tblownersettlement_settlementid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE branch.tblownersettlement_settlementid_seq OWNER TO postgres;

--
-- TOC entry 5190 (class 0 OID 0)
-- Dependencies: 259
-- Name: tblownersettlement_settlementid_seq; Type: SEQUENCE OWNED BY; Schema: branch; Owner: postgres
--

ALTER SEQUENCE branch.tblownersettlement_settlementid_seq OWNED BY branch.tblownersettlement.settlementid;


--
-- TOC entry 252 (class 1259 OID 24891)
-- Name: tblcard; Type: TABLE; Schema: card; Owner: postgres
--

CREATE TABLE card.tblcard (
    cardid integer NOT NULL,
    userid integer,
    availableamount numeric(18,2) DEFAULT 0.00,
    lasttransactionid character varying(50),
    currentsessionid integer,
    currentcheckinstopid integer,
    checkinat timestamp without time zone,
    isactive boolean DEFAULT true,
    lastuse timestamp without time zone,
    deactivatedat timestamp without time zone,
    sectorkey bytea
);


ALTER TABLE card.tblcard OWNER TO postgres;

--
-- TOC entry 251 (class 1259 OID 24890)
-- Name: tblcard_cardid_seq; Type: SEQUENCE; Schema: card; Owner: postgres
--

CREATE SEQUENCE card.tblcard_cardid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE card.tblcard_cardid_seq OWNER TO postgres;

--
-- TOC entry 5191 (class 0 OID 0)
-- Dependencies: 251
-- Name: tblcard_cardid_seq; Type: SEQUENCE OWNED BY; Schema: card; Owner: postgres
--

ALTER SEQUENCE card.tblcard_cardid_seq OWNED BY card.tblcard.cardid;


--
-- TOC entry 264 (class 1259 OID 40967)
-- Name: tblcard_cardid_seq1; Type: SEQUENCE; Schema: card; Owner: postgres
--

ALTER TABLE card.tblcard ALTER COLUMN cardid ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME card.tblcard_cardid_seq1
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 254 (class 1259 OID 24901)
-- Name: tblcardhistory; Type: TABLE; Schema: card; Owner: postgres
--

CREATE TABLE card.tblcardhistory (
    id integer NOT NULL,
    cardid integer,
    userid integer,
    transactionat timestamp without time zone,
    payid integer,
    rechargeid integer,
    amount numeric(18,2),
    balanceafter numeric(18,2),
    transactiontype character varying(20)
);


ALTER TABLE card.tblcardhistory OWNER TO postgres;

--
-- TOC entry 253 (class 1259 OID 24900)
-- Name: tblcardhistory_id_seq; Type: SEQUENCE; Schema: card; Owner: postgres
--

CREATE SEQUENCE card.tblcardhistory_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE card.tblcardhistory_id_seq OWNER TO postgres;

--
-- TOC entry 5192 (class 0 OID 0)
-- Dependencies: 253
-- Name: tblcardhistory_id_seq; Type: SEQUENCE OWNED BY; Schema: card; Owner: postgres
--

ALTER SEQUENCE card.tblcardhistory_id_seq OWNED BY card.tblcardhistory.id;


--
-- TOC entry 234 (class 1259 OID 24815)
-- Name: tblentity; Type: TABLE; Schema: entity; Owner: postgres
--

CREATE TABLE entity.tblentity (
    entityid integer NOT NULL,
    entityname character varying(200),
    ownerid integer,
    createdat timestamp without time zone,
    entitylocation character varying(200),
    isactive boolean
);


ALTER TABLE entity.tblentity OWNER TO postgres;

--
-- TOC entry 233 (class 1259 OID 24814)
-- Name: tblentity_entityid_seq; Type: SEQUENCE; Schema: entity; Owner: postgres
--

CREATE SEQUENCE entity.tblentity_entityid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE entity.tblentity_entityid_seq OWNER TO postgres;

--
-- TOC entry 5193 (class 0 OID 0)
-- Dependencies: 233
-- Name: tblentity_entityid_seq; Type: SEQUENCE OWNED BY; Schema: entity; Owner: postgres
--

ALTER SEQUENCE entity.tblentity_entityid_seq OWNED BY entity.tblentity.entityid;


--
-- TOC entry 232 (class 1259 OID 24805)
-- Name: tblentityowner; Type: TABLE; Schema: entity; Owner: postgres
--

CREATE TABLE entity.tblentityowner (
    ownerid integer NOT NULL,
    address_1 character varying(200),
    address_2 character varying(200),
    name character varying(100),
    phone character varying(100),
    createdat timestamp without time zone,
    isactive boolean
);


ALTER TABLE entity.tblentityowner OWNER TO postgres;

--
-- TOC entry 231 (class 1259 OID 24804)
-- Name: tblentityowner_ownerid_seq; Type: SEQUENCE; Schema: entity; Owner: postgres
--

CREATE SEQUENCE entity.tblentityowner_ownerid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE entity.tblentityowner_ownerid_seq OWNER TO postgres;

--
-- TOC entry 5194 (class 0 OID 0)
-- Dependencies: 231
-- Name: tblentityowner_ownerid_seq; Type: SEQUENCE OWNED BY; Schema: entity; Owner: postgres
--

ALTER SEQUENCE entity.tblentityowner_ownerid_seq OWNED BY entity.tblentityowner.ownerid;


--
-- TOC entry 246 (class 1259 OID 24865)
-- Name: tblbussession; Type: TABLE; Schema: machine; Owner: postgres
--

CREATE TABLE machine.tblbussession (
    sessionid integer NOT NULL,
    machineid integer,
    routeid integer,
    driverid integer,
    startedat timestamp without time zone,
    endedat timestamp without time zone,
    currentstopid integer,
    status character varying(20)
);


ALTER TABLE machine.tblbussession OWNER TO postgres;

--
-- TOC entry 245 (class 1259 OID 24864)
-- Name: tblbussession_sessionid_seq; Type: SEQUENCE; Schema: machine; Owner: postgres
--

CREATE SEQUENCE machine.tblbussession_sessionid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE machine.tblbussession_sessionid_seq OWNER TO postgres;

--
-- TOC entry 5195 (class 0 OID 0)
-- Dependencies: 245
-- Name: tblbussession_sessionid_seq; Type: SEQUENCE OWNED BY; Schema: machine; Owner: postgres
--

ALTER SEQUENCE machine.tblbussession_sessionid_seq OWNED BY machine.tblbussession.sessionid;


--
-- TOC entry 244 (class 1259 OID 24855)
-- Name: tblmachine; Type: TABLE; Schema: machine; Owner: postgres
--

CREATE TABLE machine.tblmachine (
    machineid integer NOT NULL,
    entityid integer,
    apikey bytea,
    apikeyexpiresat timestamp without time zone,
    machinelocation character varying(200),
    isactive boolean,
    lastauthat timestamp without time zone,
    lastsyncat timestamp without time zone
);


ALTER TABLE machine.tblmachine OWNER TO postgres;

--
-- TOC entry 243 (class 1259 OID 24854)
-- Name: tblmachine_machineid_seq; Type: SEQUENCE; Schema: machine; Owner: postgres
--

CREATE SEQUENCE machine.tblmachine_machineid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE machine.tblmachine_machineid_seq OWNER TO postgres;

--
-- TOC entry 5196 (class 0 OID 0)
-- Dependencies: 243
-- Name: tblmachine_machineid_seq; Type: SEQUENCE OWNED BY; Schema: machine; Owner: postgres
--

ALTER SEQUENCE machine.tblmachine_machineid_seq OWNED BY machine.tblmachine.machineid;


--
-- TOC entry 268 (class 1259 OID 41003)
-- Name: tblmenurole; Type: TABLE; Schema: permission; Owner: postgres
--

CREATE TABLE permission.tblmenurole (
    id integer NOT NULL,
    menuid integer,
    roleid integer
);


ALTER TABLE permission.tblmenurole OWNER TO postgres;

--
-- TOC entry 267 (class 1259 OID 41002)
-- Name: tblmenurole_id_seq; Type: SEQUENCE; Schema: permission; Owner: postgres
--

CREATE SEQUENCE permission.tblmenurole_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE permission.tblmenurole_id_seq OWNER TO postgres;

--
-- TOC entry 5197 (class 0 OID 0)
-- Dependencies: 267
-- Name: tblmenurole_id_seq; Type: SEQUENCE OWNED BY; Schema: permission; Owner: postgres
--

ALTER SEQUENCE permission.tblmenurole_id_seq OWNED BY permission.tblmenurole.id;


--
-- TOC entry 229 (class 1259 OID 24791)
-- Name: tblpermission; Type: TABLE; Schema: permission; Owner: postgres
--

CREATE TABLE permission.tblpermission (
    permid integer NOT NULL,
    permkey character varying(20),
    label character varying(20)
);


ALTER TABLE permission.tblpermission OWNER TO postgres;

--
-- TOC entry 263 (class 1259 OID 32773)
-- Name: tblpermission_permid_seq; Type: SEQUENCE; Schema: permission; Owner: postgres
--

ALTER TABLE permission.tblpermission ALTER COLUMN permid ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME permission.tblpermission_permid_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 230 (class 1259 OID 24797)
-- Name: tblrolepermission; Type: TABLE; Schema: permission; Owner: postgres
--

CREATE TABLE permission.tblrolepermission (
    roleid integer NOT NULL,
    permid integer NOT NULL,
    isallowed boolean
);


ALTER TABLE permission.tblrolepermission OWNER TO postgres;

--
-- TOC entry 262 (class 1259 OID 24983)
-- Name: tblroles; Type: TABLE; Schema: permission; Owner: postgres
--

CREATE TABLE permission.tblroles (
    roleid integer NOT NULL,
    rolename character varying(50)
);


ALTER TABLE permission.tblroles OWNER TO postgres;

--
-- TOC entry 261 (class 1259 OID 24982)
-- Name: tblroles_roleid_seq; Type: SEQUENCE; Schema: permission; Owner: postgres
--

CREATE SEQUENCE permission.tblroles_roleid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE permission.tblroles_roleid_seq OWNER TO postgres;

--
-- TOC entry 5198 (class 0 OID 0)
-- Dependencies: 261
-- Name: tblroles_roleid_seq; Type: SEQUENCE OWNED BY; Schema: permission; Owner: postgres
--

ALTER SEQUENCE permission.tblroles_roleid_seq OWNED BY permission.tblroles.roleid;


--
-- TOC entry 266 (class 1259 OID 40990)
-- Name: tblmenulist; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tblmenulist (
    menuid integer NOT NULL,
    menuname character varying(100) NOT NULL,
    parentid integer DEFAULT 0 NOT NULL,
    icon character varying(50),
    path character varying(255),
    menuorder integer DEFAULT 0 NOT NULL,
    createdby integer,
    childid integer
);


ALTER TABLE public.tblmenulist OWNER TO postgres;

--
-- TOC entry 265 (class 1259 OID 40989)
-- Name: tblmenulist_menuid_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tblmenulist_menuid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.tblmenulist_menuid_seq OWNER TO postgres;

--
-- TOC entry 5199 (class 0 OID 0)
-- Dependencies: 265
-- Name: tblmenulist_menuid_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tblmenulist_menuid_seq OWNED BY public.tblmenulist.menuid;


--
-- TOC entry 242 (class 1259 OID 24847)
-- Name: tblfarerule; Type: TABLE; Schema: route; Owner: postgres
--

CREATE TABLE route.tblfarerule (
    fareid integer NOT NULL,
    routeid integer,
    fromstopid integer,
    tostopid integer,
    fare numeric(18,2)
);


ALTER TABLE route.tblfarerule OWNER TO postgres;

--
-- TOC entry 241 (class 1259 OID 24846)
-- Name: tblfarerule_fareid_seq; Type: SEQUENCE; Schema: route; Owner: postgres
--

CREATE SEQUENCE route.tblfarerule_fareid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE route.tblfarerule_fareid_seq OWNER TO postgres;

--
-- TOC entry 5200 (class 0 OID 0)
-- Dependencies: 241
-- Name: tblfarerule_fareid_seq; Type: SEQUENCE OWNED BY; Schema: route; Owner: postgres
--

ALTER SEQUENCE route.tblfarerule_fareid_seq OWNED BY route.tblfarerule.fareid;


--
-- TOC entry 238 (class 1259 OID 24831)
-- Name: tblroute; Type: TABLE; Schema: route; Owner: postgres
--

CREATE TABLE route.tblroute (
    routeid integer NOT NULL,
    routename character varying(100),
    entityid integer,
    isactive boolean
);


ALTER TABLE route.tblroute OWNER TO postgres;

--
-- TOC entry 237 (class 1259 OID 24830)
-- Name: tblroute_routeid_seq; Type: SEQUENCE; Schema: route; Owner: postgres
--

CREATE SEQUENCE route.tblroute_routeid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE route.tblroute_routeid_seq OWNER TO postgres;

--
-- TOC entry 5201 (class 0 OID 0)
-- Dependencies: 237
-- Name: tblroute_routeid_seq; Type: SEQUENCE OWNED BY; Schema: route; Owner: postgres
--

ALTER SEQUENCE route.tblroute_routeid_seq OWNED BY route.tblroute.routeid;


--
-- TOC entry 240 (class 1259 OID 24839)
-- Name: tblroutestop; Type: TABLE; Schema: route; Owner: postgres
--

CREATE TABLE route.tblroutestop (
    routestopid integer NOT NULL,
    routeid integer,
    stopid integer,
    stoporder integer
);


ALTER TABLE route.tblroutestop OWNER TO postgres;

--
-- TOC entry 239 (class 1259 OID 24838)
-- Name: tblroutestop_routestopid_seq; Type: SEQUENCE; Schema: route; Owner: postgres
--

CREATE SEQUENCE route.tblroutestop_routestopid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE route.tblroutestop_routestopid_seq OWNER TO postgres;

--
-- TOC entry 5202 (class 0 OID 0)
-- Dependencies: 239
-- Name: tblroutestop_routestopid_seq; Type: SEQUENCE OWNED BY; Schema: route; Owner: postgres
--

ALTER SEQUENCE route.tblroutestop_routestopid_seq OWNED BY route.tblroutestop.routestopid;


--
-- TOC entry 236 (class 1259 OID 24823)
-- Name: tblstop; Type: TABLE; Schema: route; Owner: postgres
--

CREATE TABLE route.tblstop (
    stopid integer NOT NULL,
    stopname character varying(100),
    isactive boolean
);


ALTER TABLE route.tblstop OWNER TO postgres;

--
-- TOC entry 235 (class 1259 OID 24822)
-- Name: tblstop_stopid_seq; Type: SEQUENCE; Schema: route; Owner: postgres
--

CREATE SEQUENCE route.tblstop_stopid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE route.tblstop_stopid_seq OWNER TO postgres;

--
-- TOC entry 5203 (class 0 OID 0)
-- Dependencies: 235
-- Name: tblstop_stopid_seq; Type: SEQUENCE OWNED BY; Schema: route; Owner: postgres
--

ALTER SEQUENCE route.tblstop_stopid_seq OWNED BY route.tblstop.stopid;


--
-- TOC entry 250 (class 1259 OID 24881)
-- Name: tblpendingsync; Type: TABLE; Schema: transaction; Owner: postgres
--

CREATE TABLE transaction.tblpendingsync (
    syncid integer NOT NULL,
    machineid integer,
    sessionid integer,
    payload text,
    receivedat timestamp without time zone,
    retrycount integer,
    lasttriedat timestamp without time zone,
    errormessage character varying(500),
    status character varying(20)
);


ALTER TABLE transaction.tblpendingsync OWNER TO postgres;

--
-- TOC entry 249 (class 1259 OID 24880)
-- Name: tblpendingsync_syncid_seq; Type: SEQUENCE; Schema: transaction; Owner: postgres
--

CREATE SEQUENCE transaction.tblpendingsync_syncid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE transaction.tblpendingsync_syncid_seq OWNER TO postgres;

--
-- TOC entry 5204 (class 0 OID 0)
-- Dependencies: 249
-- Name: tblpendingsync_syncid_seq; Type: SEQUENCE OWNED BY; Schema: transaction; Owner: postgres
--

ALTER SEQUENCE transaction.tblpendingsync_syncid_seq OWNED BY transaction.tblpendingsync.syncid;


--
-- TOC entry 248 (class 1259 OID 24873)
-- Name: tbluserpaymenthistory; Type: TABLE; Schema: transaction; Owner: postgres
--

CREATE TABLE transaction.tbluserpaymenthistory (
    payid integer NOT NULL,
    userid integer,
    machineid integer,
    checkinstopid integer,
    checkoutstopid integer,
    checkinat timestamp without time zone,
    checkoutat timestamp without time zone,
    fare numeric(18,2),
    sessionid integer,
    status character varying(20)
);


ALTER TABLE transaction.tbluserpaymenthistory OWNER TO postgres;

--
-- TOC entry 247 (class 1259 OID 24872)
-- Name: tbluserpaymenthistory_payid_seq; Type: SEQUENCE; Schema: transaction; Owner: postgres
--

CREATE SEQUENCE transaction.tbluserpaymenthistory_payid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE transaction.tbluserpaymenthistory_payid_seq OWNER TO postgres;

--
-- TOC entry 5205 (class 0 OID 0)
-- Dependencies: 247
-- Name: tbluserpaymenthistory_payid_seq; Type: SEQUENCE OWNED BY; Schema: transaction; Owner: postgres
--

ALTER SEQUENCE transaction.tbluserpaymenthistory_payid_seq OWNED BY transaction.tbluserpaymenthistory.payid;


--
-- TOC entry 228 (class 1259 OID 24775)
-- Name: tblusers; Type: TABLE; Schema: user; Owner: postgres
--

CREATE TABLE "user".tblusers (
    userid integer NOT NULL,
    rfid character varying(20),
    username character varying(100),
    roleid integer,
    name character varying(100),
    address character varying(100),
    phone character varying(20),
    isactive boolean,
    password bytea,
    createdat timestamp without time zone,
    cardid integer
);


ALTER TABLE "user".tblusers OWNER TO postgres;

--
-- TOC entry 227 (class 1259 OID 24774)
-- Name: tblusers_userid_seq; Type: SEQUENCE; Schema: user; Owner: postgres
--

CREATE SEQUENCE "user".tblusers_userid_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE "user".tblusers_userid_seq OWNER TO postgres;

--
-- TOC entry 5206 (class 0 OID 0)
-- Dependencies: 227
-- Name: tblusers_userid_seq; Type: SEQUENCE OWNED BY; Schema: user; Owner: postgres
--

ALTER SEQUENCE "user".tblusers_userid_seq OWNED BY "user".tblusers.userid;


--
-- TOC entry 4938 (class 2604 OID 24912)
-- Name: tblbranch branchid; Type: DEFAULT; Schema: branch; Owner: postgres
--

ALTER TABLE ONLY branch.tblbranch ALTER COLUMN branchid SET DEFAULT nextval('branch.tblbranch_branchid_seq'::regclass);


--
-- TOC entry 4939 (class 2604 OID 24920)
-- Name: tblcardrecharge rechargeid; Type: DEFAULT; Schema: branch; Owner: postgres
--

ALTER TABLE ONLY branch.tblcardrecharge ALTER COLUMN rechargeid SET DEFAULT nextval('branch.tblcardrecharge_rechargeid_seq'::regclass);


--
-- TOC entry 4940 (class 2604 OID 24928)
-- Name: tblownersettlement settlementid; Type: DEFAULT; Schema: branch; Owner: postgres
--

ALTER TABLE ONLY branch.tblownersettlement ALTER COLUMN settlementid SET DEFAULT nextval('branch.tblownersettlement_settlementid_seq'::regclass);


--
-- TOC entry 4937 (class 2604 OID 24904)
-- Name: tblcardhistory id; Type: DEFAULT; Schema: card; Owner: postgres
--

ALTER TABLE ONLY card.tblcardhistory ALTER COLUMN id SET DEFAULT nextval('card.tblcardhistory_id_seq'::regclass);


--
-- TOC entry 4926 (class 2604 OID 24818)
-- Name: tblentity entityid; Type: DEFAULT; Schema: entity; Owner: postgres
--

ALTER TABLE ONLY entity.tblentity ALTER COLUMN entityid SET DEFAULT nextval('entity.tblentity_entityid_seq'::regclass);


--
-- TOC entry 4925 (class 2604 OID 24808)
-- Name: tblentityowner ownerid; Type: DEFAULT; Schema: entity; Owner: postgres
--

ALTER TABLE ONLY entity.tblentityowner ALTER COLUMN ownerid SET DEFAULT nextval('entity.tblentityowner_ownerid_seq'::regclass);


--
-- TOC entry 4932 (class 2604 OID 24868)
-- Name: tblbussession sessionid; Type: DEFAULT; Schema: machine; Owner: postgres
--

ALTER TABLE ONLY machine.tblbussession ALTER COLUMN sessionid SET DEFAULT nextval('machine.tblbussession_sessionid_seq'::regclass);


--
-- TOC entry 4931 (class 2604 OID 24858)
-- Name: tblmachine machineid; Type: DEFAULT; Schema: machine; Owner: postgres
--

ALTER TABLE ONLY machine.tblmachine ALTER COLUMN machineid SET DEFAULT nextval('machine.tblmachine_machineid_seq'::regclass);


--
-- TOC entry 4945 (class 2604 OID 41006)
-- Name: tblmenurole id; Type: DEFAULT; Schema: permission; Owner: postgres
--

ALTER TABLE ONLY permission.tblmenurole ALTER COLUMN id SET DEFAULT nextval('permission.tblmenurole_id_seq'::regclass);


--
-- TOC entry 4941 (class 2604 OID 24986)
-- Name: tblroles roleid; Type: DEFAULT; Schema: permission; Owner: postgres
--

ALTER TABLE ONLY permission.tblroles ALTER COLUMN roleid SET DEFAULT nextval('permission.tblroles_roleid_seq'::regclass);


--
-- TOC entry 4942 (class 2604 OID 40993)
-- Name: tblmenulist menuid; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tblmenulist ALTER COLUMN menuid SET DEFAULT nextval('public.tblmenulist_menuid_seq'::regclass);


--
-- TOC entry 4930 (class 2604 OID 24850)
-- Name: tblfarerule fareid; Type: DEFAULT; Schema: route; Owner: postgres
--

ALTER TABLE ONLY route.tblfarerule ALTER COLUMN fareid SET DEFAULT nextval('route.tblfarerule_fareid_seq'::regclass);


--
-- TOC entry 4928 (class 2604 OID 24834)
-- Name: tblroute routeid; Type: DEFAULT; Schema: route; Owner: postgres
--

ALTER TABLE ONLY route.tblroute ALTER COLUMN routeid SET DEFAULT nextval('route.tblroute_routeid_seq'::regclass);


--
-- TOC entry 4929 (class 2604 OID 24842)
-- Name: tblroutestop routestopid; Type: DEFAULT; Schema: route; Owner: postgres
--

ALTER TABLE ONLY route.tblroutestop ALTER COLUMN routestopid SET DEFAULT nextval('route.tblroutestop_routestopid_seq'::regclass);


--
-- TOC entry 4927 (class 2604 OID 24826)
-- Name: tblstop stopid; Type: DEFAULT; Schema: route; Owner: postgres
--

ALTER TABLE ONLY route.tblstop ALTER COLUMN stopid SET DEFAULT nextval('route.tblstop_stopid_seq'::regclass);


--
-- TOC entry 4934 (class 2604 OID 24884)
-- Name: tblpendingsync syncid; Type: DEFAULT; Schema: transaction; Owner: postgres
--

ALTER TABLE ONLY transaction.tblpendingsync ALTER COLUMN syncid SET DEFAULT nextval('transaction.tblpendingsync_syncid_seq'::regclass);


--
-- TOC entry 4933 (class 2604 OID 24876)
-- Name: tbluserpaymenthistory payid; Type: DEFAULT; Schema: transaction; Owner: postgres
--

ALTER TABLE ONLY transaction.tbluserpaymenthistory ALTER COLUMN payid SET DEFAULT nextval('transaction.tbluserpaymenthistory_payid_seq'::regclass);


--
-- TOC entry 4924 (class 2604 OID 24778)
-- Name: tblusers userid; Type: DEFAULT; Schema: user; Owner: postgres
--

ALTER TABLE ONLY "user".tblusers ALTER COLUMN userid SET DEFAULT nextval('"user".tblusers_userid_seq'::regclass);


--
-- TOC entry 5169 (class 0 OID 24909)
-- Dependencies: 256
-- Data for Name: tblbranch; Type: TABLE DATA; Schema: branch; Owner: postgres
--

COPY branch.tblbranch (branchid, name, address, phone, createdat) FROM stdin;
\.


--
-- TOC entry 5171 (class 0 OID 24917)
-- Dependencies: 258
-- Data for Name: tblcardrecharge; Type: TABLE DATA; Schema: branch; Owner: postgres
--

COPY branch.tblcardrecharge (rechargeid, branchid, cardid, userid, amount, rechargedat, rechargedby) FROM stdin;
\.


--
-- TOC entry 5173 (class 0 OID 24925)
-- Dependencies: 260
-- Data for Name: tblownersettlement; Type: TABLE DATA; Schema: branch; Owner: postgres
--

COPY branch.tblownersettlement (settlementid, entityid, ownerid, branchid, amount, settledat, notes, settledby) FROM stdin;
\.


--
-- TOC entry 5165 (class 0 OID 24891)
-- Dependencies: 252
-- Data for Name: tblcard; Type: TABLE DATA; Schema: card; Owner: postgres
--

COPY card.tblcard (cardid, userid, availableamount, lasttransactionid, currentsessionid, currentcheckinstopid, checkinat, isactive, lastuse, deactivatedat, sectorkey) FROM stdin;
20	11	0.00	\N	\N	\N	\N	t	\N	\N	\N
\.


--
-- TOC entry 5167 (class 0 OID 24901)
-- Dependencies: 254
-- Data for Name: tblcardhistory; Type: TABLE DATA; Schema: card; Owner: postgres
--

COPY card.tblcardhistory (id, cardid, userid, transactionat, payid, rechargeid, amount, balanceafter, transactiontype) FROM stdin;
\.


--
-- TOC entry 5147 (class 0 OID 24815)
-- Dependencies: 234
-- Data for Name: tblentity; Type: TABLE DATA; Schema: entity; Owner: postgres
--

COPY entity.tblentity (entityid, entityname, ownerid, createdat, entitylocation, isactive) FROM stdin;
\.


--
-- TOC entry 5145 (class 0 OID 24805)
-- Dependencies: 232
-- Data for Name: tblentityowner; Type: TABLE DATA; Schema: entity; Owner: postgres
--

COPY entity.tblentityowner (ownerid, address_1, address_2, name, phone, createdat, isactive) FROM stdin;
\.


--
-- TOC entry 5159 (class 0 OID 24865)
-- Dependencies: 246
-- Data for Name: tblbussession; Type: TABLE DATA; Schema: machine; Owner: postgres
--

COPY machine.tblbussession (sessionid, machineid, routeid, driverid, startedat, endedat, currentstopid, status) FROM stdin;
\.


--
-- TOC entry 5157 (class 0 OID 24855)
-- Dependencies: 244
-- Data for Name: tblmachine; Type: TABLE DATA; Schema: machine; Owner: postgres
--

COPY machine.tblmachine (machineid, entityid, apikey, apikeyexpiresat, machinelocation, isactive, lastauthat, lastsyncat) FROM stdin;
\.


--
-- TOC entry 5181 (class 0 OID 41003)
-- Dependencies: 268
-- Data for Name: tblmenurole; Type: TABLE DATA; Schema: permission; Owner: postgres
--

COPY permission.tblmenurole (id, menuid, roleid) FROM stdin;
1	1	1
2	2	1
3	3	1
4	4	1
5	5	1
\.


--
-- TOC entry 5142 (class 0 OID 24791)
-- Dependencies: 229
-- Data for Name: tblpermission; Type: TABLE DATA; Schema: permission; Owner: postgres
--

COPY permission.tblpermission (permid, permkey, label) FROM stdin;
1	USR.C	\N
2	USR.C	\N
\.


--
-- TOC entry 5143 (class 0 OID 24797)
-- Dependencies: 230
-- Data for Name: tblrolepermission; Type: TABLE DATA; Schema: permission; Owner: postgres
--

COPY permission.tblrolepermission (roleid, permid, isallowed) FROM stdin;
1	1	t
1	2	t
\.


--
-- TOC entry 5175 (class 0 OID 24983)
-- Dependencies: 262
-- Data for Name: tblroles; Type: TABLE DATA; Schema: permission; Owner: postgres
--

COPY permission.tblroles (roleid, rolename) FROM stdin;
1	admin
34	Standard User
\.


--
-- TOC entry 5179 (class 0 OID 40990)
-- Dependencies: 266
-- Data for Name: tblmenulist; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.tblmenulist (menuid, menuname, parentid, icon, path, menuorder, createdby, childid) FROM stdin;
1	Dashboard	0	dashboard	/	0	\N	\N
2	Menu Setup	1	menu	/menusetup	1	\N	\N
3	User Report	1	report	/userreport	2	\N	\N
5	Assign Card	4	Card	/assigncard	1	\N	\N
4	Card	0	Card	/	0	\N	\N
\.


--
-- TOC entry 5155 (class 0 OID 24847)
-- Dependencies: 242
-- Data for Name: tblfarerule; Type: TABLE DATA; Schema: route; Owner: postgres
--

COPY route.tblfarerule (fareid, routeid, fromstopid, tostopid, fare) FROM stdin;
\.


--
-- TOC entry 5151 (class 0 OID 24831)
-- Dependencies: 238
-- Data for Name: tblroute; Type: TABLE DATA; Schema: route; Owner: postgres
--

COPY route.tblroute (routeid, routename, entityid, isactive) FROM stdin;
\.


--
-- TOC entry 5153 (class 0 OID 24839)
-- Dependencies: 240
-- Data for Name: tblroutestop; Type: TABLE DATA; Schema: route; Owner: postgres
--

COPY route.tblroutestop (routestopid, routeid, stopid, stoporder) FROM stdin;
\.


--
-- TOC entry 5149 (class 0 OID 24823)
-- Dependencies: 236
-- Data for Name: tblstop; Type: TABLE DATA; Schema: route; Owner: postgres
--

COPY route.tblstop (stopid, stopname, isactive) FROM stdin;
\.


--
-- TOC entry 5163 (class 0 OID 24881)
-- Dependencies: 250
-- Data for Name: tblpendingsync; Type: TABLE DATA; Schema: transaction; Owner: postgres
--

COPY transaction.tblpendingsync (syncid, machineid, sessionid, payload, receivedat, retrycount, lasttriedat, errormessage, status) FROM stdin;
\.


--
-- TOC entry 5161 (class 0 OID 24873)
-- Dependencies: 248
-- Data for Name: tbluserpaymenthistory; Type: TABLE DATA; Schema: transaction; Owner: postgres
--

COPY transaction.tbluserpaymenthistory (payid, userid, machineid, checkinstopid, checkoutstopid, checkinat, checkoutat, fare, sessionid, status) FROM stdin;
\.


--
-- TOC entry 5141 (class 0 OID 24775)
-- Dependencies: 228
-- Data for Name: tblusers; Type: TABLE DATA; Schema: user; Owner: postgres
--

COPY "user".tblusers (userid, rfid, username, roleid, name, address, phone, isactive, password, createdat, cardid) FROM stdin;
11	\N	admin	1	Admin User	tokha-3, ktm	9812345678	t	\\x24326124313124675669334c316736486657614132727156634f7a712e6e73507978585830306e33423251732e576b30786d3339704553506e67674b	2026-06-14 14:36:38.729428	20
\.


--
-- TOC entry 5207 (class 0 OID 0)
-- Dependencies: 255
-- Name: tblbranch_branchid_seq; Type: SEQUENCE SET; Schema: branch; Owner: postgres
--

SELECT pg_catalog.setval('branch.tblbranch_branchid_seq', 1, false);


--
-- TOC entry 5208 (class 0 OID 0)
-- Dependencies: 257
-- Name: tblcardrecharge_rechargeid_seq; Type: SEQUENCE SET; Schema: branch; Owner: postgres
--

SELECT pg_catalog.setval('branch.tblcardrecharge_rechargeid_seq', 1, false);


--
-- TOC entry 5209 (class 0 OID 0)
-- Dependencies: 259
-- Name: tblownersettlement_settlementid_seq; Type: SEQUENCE SET; Schema: branch; Owner: postgres
--

SELECT pg_catalog.setval('branch.tblownersettlement_settlementid_seq', 1, false);


--
-- TOC entry 5210 (class 0 OID 0)
-- Dependencies: 251
-- Name: tblcard_cardid_seq; Type: SEQUENCE SET; Schema: card; Owner: postgres
--

SELECT pg_catalog.setval('card.tblcard_cardid_seq', 1, false);


--
-- TOC entry 5211 (class 0 OID 0)
-- Dependencies: 264
-- Name: tblcard_cardid_seq1; Type: SEQUENCE SET; Schema: card; Owner: postgres
--

SELECT pg_catalog.setval('card.tblcard_cardid_seq1', 20, true);


--
-- TOC entry 5212 (class 0 OID 0)
-- Dependencies: 253
-- Name: tblcardhistory_id_seq; Type: SEQUENCE SET; Schema: card; Owner: postgres
--

SELECT pg_catalog.setval('card.tblcardhistory_id_seq', 1, false);


--
-- TOC entry 5213 (class 0 OID 0)
-- Dependencies: 233
-- Name: tblentity_entityid_seq; Type: SEQUENCE SET; Schema: entity; Owner: postgres
--

SELECT pg_catalog.setval('entity.tblentity_entityid_seq', 1, false);


--
-- TOC entry 5214 (class 0 OID 0)
-- Dependencies: 231
-- Name: tblentityowner_ownerid_seq; Type: SEQUENCE SET; Schema: entity; Owner: postgres
--

SELECT pg_catalog.setval('entity.tblentityowner_ownerid_seq', 1, false);


--
-- TOC entry 5215 (class 0 OID 0)
-- Dependencies: 245
-- Name: tblbussession_sessionid_seq; Type: SEQUENCE SET; Schema: machine; Owner: postgres
--

SELECT pg_catalog.setval('machine.tblbussession_sessionid_seq', 1, false);


--
-- TOC entry 5216 (class 0 OID 0)
-- Dependencies: 243
-- Name: tblmachine_machineid_seq; Type: SEQUENCE SET; Schema: machine; Owner: postgres
--

SELECT pg_catalog.setval('machine.tblmachine_machineid_seq', 1, false);


--
-- TOC entry 5217 (class 0 OID 0)
-- Dependencies: 267
-- Name: tblmenurole_id_seq; Type: SEQUENCE SET; Schema: permission; Owner: postgres
--

SELECT pg_catalog.setval('permission.tblmenurole_id_seq', 5, true);


--
-- TOC entry 5218 (class 0 OID 0)
-- Dependencies: 263
-- Name: tblpermission_permid_seq; Type: SEQUENCE SET; Schema: permission; Owner: postgres
--

SELECT pg_catalog.setval('permission.tblpermission_permid_seq', 2, true);


--
-- TOC entry 5219 (class 0 OID 0)
-- Dependencies: 261
-- Name: tblroles_roleid_seq; Type: SEQUENCE SET; Schema: permission; Owner: postgres
--

SELECT pg_catalog.setval('permission.tblroles_roleid_seq', 37, true);


--
-- TOC entry 5220 (class 0 OID 0)
-- Dependencies: 265
-- Name: tblmenulist_menuid_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.tblmenulist_menuid_seq', 5, true);


--
-- TOC entry 5221 (class 0 OID 0)
-- Dependencies: 241
-- Name: tblfarerule_fareid_seq; Type: SEQUENCE SET; Schema: route; Owner: postgres
--

SELECT pg_catalog.setval('route.tblfarerule_fareid_seq', 1, false);


--
-- TOC entry 5222 (class 0 OID 0)
-- Dependencies: 237
-- Name: tblroute_routeid_seq; Type: SEQUENCE SET; Schema: route; Owner: postgres
--

SELECT pg_catalog.setval('route.tblroute_routeid_seq', 1, false);


--
-- TOC entry 5223 (class 0 OID 0)
-- Dependencies: 239
-- Name: tblroutestop_routestopid_seq; Type: SEQUENCE SET; Schema: route; Owner: postgres
--

SELECT pg_catalog.setval('route.tblroutestop_routestopid_seq', 1, false);


--
-- TOC entry 5224 (class 0 OID 0)
-- Dependencies: 235
-- Name: tblstop_stopid_seq; Type: SEQUENCE SET; Schema: route; Owner: postgres
--

SELECT pg_catalog.setval('route.tblstop_stopid_seq', 1, false);


--
-- TOC entry 5225 (class 0 OID 0)
-- Dependencies: 249
-- Name: tblpendingsync_syncid_seq; Type: SEQUENCE SET; Schema: transaction; Owner: postgres
--

SELECT pg_catalog.setval('transaction.tblpendingsync_syncid_seq', 1, false);


--
-- TOC entry 5226 (class 0 OID 0)
-- Dependencies: 247
-- Name: tbluserpaymenthistory_payid_seq; Type: SEQUENCE SET; Schema: transaction; Owner: postgres
--

SELECT pg_catalog.setval('transaction.tbluserpaymenthistory_payid_seq', 1, false);


--
-- TOC entry 5227 (class 0 OID 0)
-- Dependencies: 227
-- Name: tblusers_userid_seq; Type: SEQUENCE SET; Schema: user; Owner: postgres
--

SELECT pg_catalog.setval('"user".tblusers_userid_seq', 14, true);


--
-- TOC entry 4982 (class 2606 OID 24915)
-- Name: tblbranch tblbranch_pkey; Type: CONSTRAINT; Schema: branch; Owner: postgres
--

ALTER TABLE ONLY branch.tblbranch
    ADD CONSTRAINT tblbranch_pkey PRIMARY KEY (branchid);


--
-- TOC entry 4984 (class 2606 OID 24923)
-- Name: tblcardrecharge tblcardrecharge_pkey; Type: CONSTRAINT; Schema: branch; Owner: postgres
--

ALTER TABLE ONLY branch.tblcardrecharge
    ADD CONSTRAINT tblcardrecharge_pkey PRIMARY KEY (rechargeid);


--
-- TOC entry 4986 (class 2606 OID 24933)
-- Name: tblownersettlement tblownersettlement_pkey; Type: CONSTRAINT; Schema: branch; Owner: postgres
--

ALTER TABLE ONLY branch.tblownersettlement
    ADD CONSTRAINT tblownersettlement_pkey PRIMARY KEY (settlementid);


--
-- TOC entry 4978 (class 2606 OID 24899)
-- Name: tblcard tblcard_pkey; Type: CONSTRAINT; Schema: card; Owner: postgres
--

ALTER TABLE ONLY card.tblcard
    ADD CONSTRAINT tblcard_pkey PRIMARY KEY (cardid);


--
-- TOC entry 4980 (class 2606 OID 24907)
-- Name: tblcardhistory tblcardhistory_pkey; Type: CONSTRAINT; Schema: card; Owner: postgres
--

ALTER TABLE ONLY card.tblcardhistory
    ADD CONSTRAINT tblcardhistory_pkey PRIMARY KEY (id);


--
-- TOC entry 4960 (class 2606 OID 24821)
-- Name: tblentity tblentity_pkey; Type: CONSTRAINT; Schema: entity; Owner: postgres
--

ALTER TABLE ONLY entity.tblentity
    ADD CONSTRAINT tblentity_pkey PRIMARY KEY (entityid);


--
-- TOC entry 4958 (class 2606 OID 24813)
-- Name: tblentityowner tblentityowner_pkey; Type: CONSTRAINT; Schema: entity; Owner: postgres
--

ALTER TABLE ONLY entity.tblentityowner
    ADD CONSTRAINT tblentityowner_pkey PRIMARY KEY (ownerid);


--
-- TOC entry 4972 (class 2606 OID 24871)
-- Name: tblbussession tblbussession_pkey; Type: CONSTRAINT; Schema: machine; Owner: postgres
--

ALTER TABLE ONLY machine.tblbussession
    ADD CONSTRAINT tblbussession_pkey PRIMARY KEY (sessionid);


--
-- TOC entry 4970 (class 2606 OID 24863)
-- Name: tblmachine tblmachine_pkey; Type: CONSTRAINT; Schema: machine; Owner: postgres
--

ALTER TABLE ONLY machine.tblmachine
    ADD CONSTRAINT tblmachine_pkey PRIMARY KEY (machineid);


--
-- TOC entry 4992 (class 2606 OID 41009)
-- Name: tblmenurole tblmenurole_pkey; Type: CONSTRAINT; Schema: permission; Owner: postgres
--

ALTER TABLE ONLY permission.tblmenurole
    ADD CONSTRAINT tblmenurole_pkey PRIMARY KEY (id);


--
-- TOC entry 4954 (class 2606 OID 24796)
-- Name: tblpermission tblpermission_pkey; Type: CONSTRAINT; Schema: permission; Owner: postgres
--

ALTER TABLE ONLY permission.tblpermission
    ADD CONSTRAINT tblpermission_pkey PRIMARY KEY (permid);


--
-- TOC entry 4956 (class 2606 OID 24803)
-- Name: tblrolepermission tblrolepermission_pkey; Type: CONSTRAINT; Schema: permission; Owner: postgres
--

ALTER TABLE ONLY permission.tblrolepermission
    ADD CONSTRAINT tblrolepermission_pkey PRIMARY KEY (roleid, permid);


--
-- TOC entry 4988 (class 2606 OID 24989)
-- Name: tblroles tblroles_pkey; Type: CONSTRAINT; Schema: permission; Owner: postgres
--

ALTER TABLE ONLY permission.tblroles
    ADD CONSTRAINT tblroles_pkey PRIMARY KEY (roleid);


--
-- TOC entry 4990 (class 2606 OID 41001)
-- Name: tblmenulist tblmenulist_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tblmenulist
    ADD CONSTRAINT tblmenulist_pkey PRIMARY KEY (menuid);


--
-- TOC entry 4968 (class 2606 OID 24853)
-- Name: tblfarerule tblfarerule_pkey; Type: CONSTRAINT; Schema: route; Owner: postgres
--

ALTER TABLE ONLY route.tblfarerule
    ADD CONSTRAINT tblfarerule_pkey PRIMARY KEY (fareid);


--
-- TOC entry 4964 (class 2606 OID 24837)
-- Name: tblroute tblroute_pkey; Type: CONSTRAINT; Schema: route; Owner: postgres
--

ALTER TABLE ONLY route.tblroute
    ADD CONSTRAINT tblroute_pkey PRIMARY KEY (routeid);


--
-- TOC entry 4966 (class 2606 OID 24845)
-- Name: tblroutestop tblroutestop_pkey; Type: CONSTRAINT; Schema: route; Owner: postgres
--

ALTER TABLE ONLY route.tblroutestop
    ADD CONSTRAINT tblroutestop_pkey PRIMARY KEY (routestopid);


--
-- TOC entry 4962 (class 2606 OID 24829)
-- Name: tblstop tblstop_pkey; Type: CONSTRAINT; Schema: route; Owner: postgres
--

ALTER TABLE ONLY route.tblstop
    ADD CONSTRAINT tblstop_pkey PRIMARY KEY (stopid);


--
-- TOC entry 4976 (class 2606 OID 24889)
-- Name: tblpendingsync tblpendingsync_pkey; Type: CONSTRAINT; Schema: transaction; Owner: postgres
--

ALTER TABLE ONLY transaction.tblpendingsync
    ADD CONSTRAINT tblpendingsync_pkey PRIMARY KEY (syncid);


--
-- TOC entry 4974 (class 2606 OID 24879)
-- Name: tbluserpaymenthistory tbluserpaymenthistory_pkey; Type: CONSTRAINT; Schema: transaction; Owner: postgres
--

ALTER TABLE ONLY transaction.tbluserpaymenthistory
    ADD CONSTRAINT tbluserpaymenthistory_pkey PRIMARY KEY (payid);


--
-- TOC entry 4948 (class 2606 OID 24783)
-- Name: tblusers tblusers_pkey; Type: CONSTRAINT; Schema: user; Owner: postgres
--

ALTER TABLE ONLY "user".tblusers
    ADD CONSTRAINT tblusers_pkey PRIMARY KEY (userid);


--
-- TOC entry 4950 (class 2606 OID 24974)
-- Name: tblusers uq_tblusers_phone; Type: CONSTRAINT; Schema: user; Owner: postgres
--

ALTER TABLE ONLY "user".tblusers
    ADD CONSTRAINT uq_tblusers_phone UNIQUE (phone);


--
-- TOC entry 4952 (class 2606 OID 24972)
-- Name: tblusers uq_tblusers_username; Type: CONSTRAINT; Schema: user; Owner: postgres
--

ALTER TABLE ONLY "user".tblusers
    ADD CONSTRAINT uq_tblusers_username UNIQUE (username);


--
-- TOC entry 4946 (class 1259 OID 24784)
-- Name: ix_tblusers_rfid; Type: INDEX; Schema: user; Owner: postgres
--

CREATE UNIQUE INDEX ix_tblusers_rfid ON "user".tblusers USING btree (rfid);


-- Completed on 2026-08-27 09:54:07

--
-- PostgreSQL database dump complete
--

\unrestrict sIDEQUZLFGzZW2tVlbjRZyTmgI9Pcs4knpeu82F9MbLOc2VHwhQLnkJLMyzFlc5

--
-- Database "postgres" dump
--

\connect postgres

--
-- PostgreSQL database dump
--

\restrict UYGzNYZgxRBhRjjru07ahtjHjaouh4bMIKhSEct5d3Q2aYyNF7jD0IzwKcWw6Zw

-- Dumped from database version 18.4
-- Dumped by pg_dump version 18.4

-- Started on 2026-08-27 09:54:08

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

-- Completed on 2026-08-27 09:54:08

--
-- PostgreSQL database dump complete
--

\unrestrict UYGzNYZgxRBhRjjru07ahtjHjaouh4bMIKhSEct5d3Q2aYyNF7jD0IzwKcWw6Zw

-- Completed on 2026-08-27 09:54:08

--
-- PostgreSQL database cluster dump complete
--

