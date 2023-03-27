import React from 'react';
import { ComponentTitleWrapper } from './pageHeader.style';
import {Icon} from 'antd';

export default props =>
  <ComponentTitleWrapper className="isoComponentTitle">
    <i className="ion-ios-paper" style={{padding:"0 8px"}}/>
    {props.children}
  </ComponentTitleWrapper>;
