import styled from 'styled-components';
import WithDirection from '../../settings/withDirection';

const WDComponentDivAction = styled.div`
  text-align: right;
  display: inline-block;
  flex: 1;
  padding: 6px 20px 0 20px;
  
  button {
    margin: 0 5px;
  }
  /*@media only screen and (max-width: 1336px) {
    text-align: left;
    display: block;
    flex: none;
    width: 100%;
    padding: 0 0 10px 20px;
  }*/
 
`;

const ComponentDivAction = WithDirection(WDComponentDivAction);
export {ComponentDivAction};