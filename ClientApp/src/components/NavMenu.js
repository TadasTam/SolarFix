import React, { Component } from "react";
import { Collapse, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from "reactstrap";
import { Link } from "react-router-dom";
import { LoginMenu } from "./api-authorization/LoginMenu";
import "./NavMenu.css";

export class NavMenu extends Component {
    static displayName = NavMenu.name;

    constructor(props) {
        super(props);

        this.toggleNavbar = this.toggleNavbar.bind(this);
        this.state = {
            collapsed: true,
        };
    }

    toggleNavbar() {
        this.setState({
            collapsed: !this.state.collapsed,
        });
    }

    render() {
        return (
            <header>
                <Navbar
                    className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3"
                    container-fluid
                    light>
                    <NavbarBrand
                        tag={Link}
                        to="/">
                        <img
                            src="/solarfix-logo.png"
                            className="d-inline-block align-top"
                            style={{ width: "50px", height: "auto" }}
                            alt="Logo"
                        />
                        <span className="logo-text">SolarFix</span>
                    </NavbarBrand>
                    <NavbarToggler
                        onClick={this.toggleNavbar}
                        className="mr-2"
                    />
                    <Collapse
                        className="d-sm-inline-flex flex-sm-row-reverse"
                        isOpen={!this.state.collapsed}
                        navbar>
                        <ul className="navbar-nav flex-grow">
                            {/* <NavItem>
                                <NavLink
                                    tag={Link}
                                    className="text-dark"
                                    to="/UpdateDatabase">
                                    Update Database
                                </NavLink>
                            </NavItem> */}
                            {/* <NavItem>
                                <NavLink
                                    tag={Link}
                                    className="text-dark"
                                    to="/Monitoring">
                                    Monitoring
                                </NavLink>
                            </NavItem> */}
                            <NavItem>
                                <NavLink
                                    tag={Link}
                                    className="text-dark nav-item"
                                    to="/"
                                >
                                    Graphs
                                    <div className="popup-menu">
                                        <NavLink
                                            tag={Link}
                                            className="text-dark"
                                            to="/CloudCoverAndCurrentPowerChartPage"
                                        >
                                            Cloud Cover And Current Power
                                        </NavLink>
                                        <NavLink
                                            tag={Link}
                                            className="text-dark"
                                            to="/CloudCoverAndCurrentPowerInvertersChartPage"
                                        >
                                            Cloud Cover And Current Power Inverters Chart
                                        </NavLink>
                                        <NavLink
                                            tag={Link}
                                            className="text-dark"
                                            to="/CloudCoverAverageAndCurrentPowerPage"
                                        >
                                            Cloud Cover Average And Current Power
                                        </NavLink>
                                        <NavLink
                                            tag={Link}
                                            className="text-dark"
                                            to="/DayProducedEnergyChartPage"
                                        >
                                            Day Produced Energy
                                        </NavLink>
                                    </div>
                                </NavLink>
                            </NavItem>
                            <NavItem>
                                <NavLink
                                    tag={Link}
                                    className="text-dark nav-item"
                                    to="/"
                                >
                                    Tables
                                    <div className="popup-menu">
                                        <NavLink
                                            tag={Link}
                                            className="text-dark"
                                            to="/InverterDataTable"
                                        >
                                            Inverter Data Table
                                        </NavLink>
                                    </div>
                                </NavLink>
                            </NavItem>
                            <LoginMenu></LoginMenu>

                        </ul>
                    </Collapse>
                </Navbar>
            </header>
        );
    }
}
