import React from "react";
import { LayoutContentWrapper } from "./layoutWrapper.style";
import logoHNUE from "../../image/logoHNUE.png"
import '../../containers/Page/style.css';
const currentYear = new Date().getFullYear();
export default props => (
  <LayoutContentWrapper
    className={
      props.className != null
        ? `${props.className} isoLayoutContentWrapper`
        : "isoLayoutContentWrapper"
    }
    {...props}
  >
    {props.children}
    {/*<div className="footer-main">*/}
    {/*  <img src={logoHNUE} alt="" width={30} style={{marginRight: 10}}/>*/}
    {/*  <i>TRƯỜNG ĐẠI HỌC SƯ PHẠM HÀ NỘI</i>*/}
    {/*</div>*/}
  </LayoutContentWrapper>
);
