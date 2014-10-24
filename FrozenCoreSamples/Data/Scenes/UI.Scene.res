<root dataType="Struct" type="Duality.Resources.Scene" id="129723834">
  <globalGravity dataType="Struct" type="OpenTK.Vector2">
    <X dataType="Float">0</X>
    <Y dataType="Float">33</Y>
  </globalGravity>
  <serializeObj dataType="Array" type="Duality.GameObject[]" id="427169525">
    <item dataType="Struct" type="Duality.GameObject" id="2626213635">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="426968753">
        <_items dataType="Array" type="Duality.Component[]" id="2741579310">
          <item dataType="Struct" type="Duality.Components.Transform" id="691561271">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <angleVel dataType="Float">0</angleVel>
            <angleVelAbs dataType="Float">0</angleVelAbs>
            <deriveAngle dataType="Bool">true</deriveAngle>
            <gameobj dataType="ObjectRef">2626213635</gameobj>
            <ignoreParent dataType="Bool">false</ignoreParent>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <parentTransform />
            <pos dataType="Struct" type="OpenTK.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">-500</Z>
            </pos>
            <posAbs dataType="Struct" type="OpenTK.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">-500</Z>
            </posAbs>
            <scale dataType="Float">1</scale>
            <scaleAbs dataType="Float">1</scaleAbs>
            <vel dataType="Struct" type="OpenTK.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">0</Z>
            </vel>
            <velAbs dataType="Struct" type="OpenTK.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">0</Z>
            </velAbs>
          </item>
          <item dataType="Struct" type="Duality.Components.Camera" id="3163489442">
            <active dataType="Bool">true</active>
            <farZ dataType="Float">10000</farZ>
            <focusDist dataType="Float">500</focusDist>
            <gameobj dataType="ObjectRef">2626213635</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <nearZ dataType="Float">0</nearZ>
            <passes dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Components.Camera+Pass]]" id="202681638">
              <_items dataType="Array" type="Duality.Components.Camera+Pass[]" id="3061882112" length="4">
                <item dataType="Struct" type="Duality.Components.Camera+Pass" id="2704304796">
                  <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">0</A>
                    <B dataType="Byte">0</B>
                    <G dataType="Byte">0</G>
                    <R dataType="Byte">0</R>
                  </clearColor>
                  <clearDepth dataType="Float">1</clearDepth>
                  <clearFlags dataType="Enum" type="Duality.Drawing.ClearFlag" name="All" value="3" />
                  <CollectDrawcalls />
                  <input />
                  <matrixMode dataType="Enum" type="Duality.Drawing.RenderMatrix" name="PerspectiveWorld" value="0" />
                  <output dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderTarget]]">
                    <contentPath />
                  </output>
                  <visibilityMask dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="AllGroups" value="2147483647" />
                </item>
                <item dataType="Struct" type="Duality.Components.Camera+Pass" id="1910088214">
                  <clearColor dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">0</A>
                    <B dataType="Byte">0</B>
                    <G dataType="Byte">0</G>
                    <R dataType="Byte">0</R>
                  </clearColor>
                  <clearDepth dataType="Float">1</clearDepth>
                  <clearFlags dataType="Enum" type="Duality.Drawing.ClearFlag" name="None" value="0" />
                  <CollectDrawcalls />
                  <input />
                  <matrixMode dataType="Enum" type="Duality.Drawing.RenderMatrix" name="OrthoScreen" value="1" />
                  <output dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.RenderTarget]]">
                    <contentPath />
                  </output>
                  <visibilityMask dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="All" value="4294967295" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
              <_version dataType="Int">2</_version>
            </passes>
            <perspective dataType="Enum" type="Duality.Drawing.PerspectiveMode" name="Parallax" value="1" />
            <visibilityMask dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="All" value="4294967295" />
          </item>
          <item dataType="Struct" type="FrozenCore.Widgets.WidgetController" id="47164325">
            <_x003C_FocusedElement_x003E_k__BackingField />
            <_x003C_HoveredElement_x003E_k__BackingField />
            <_x003C_KeyboardEnabled_x003E_k__BackingField dataType="Bool">true</_x003C_KeyboardEnabled_x003E_k__BackingField>
            <_x003C_LeftMouseKey_x003E_k__BackingField dataType="Enum" type="OpenTK.Input.Key" name="Enter" value="49" />
            <_x003C_LeftMouseKeyModifier_x003E_k__BackingField dataType="Enum" type="FrozenCore.Widgets.WidgetController+ModifierKeys" name="None" value="0" />
            <_x003C_MiddleMouseKey_x003E_k__BackingField dataType="Enum" type="OpenTK.Input.Key" name="Unknown" value="0" />
            <_x003C_MiddleMouseKeyModifier_x003E_k__BackingField dataType="Enum" type="FrozenCore.Widgets.WidgetController+ModifierKeys" name="None" value="0" />
            <_x003C_MouseEnabled_x003E_k__BackingField dataType="Bool">true</_x003C_MouseEnabled_x003E_k__BackingField>
            <_x003C_NextWidgetKey_x003E_k__BackingField dataType="Enum" type="OpenTK.Input.Key" name="Tab" value="52" />
            <_x003C_NextWidgetKeyModifier_x003E_k__BackingField dataType="Enum" type="FrozenCore.Widgets.WidgetController+ModifierKeys" name="None" value="0" />
            <_x003C_PreviousWidgetKey_x003E_k__BackingField dataType="Enum" type="OpenTK.Input.Key" name="Tab" value="52" />
            <_x003C_PreviousWidgetKeyModifier_x003E_k__BackingField dataType="Enum" type="FrozenCore.Widgets.WidgetController+ModifierKeys" name="Shift" value="48" />
            <_x003C_RightMouseKey_x003E_k__BackingField dataType="Enum" type="OpenTK.Input.Key" name="Unknown" value="0" />
            <_x003C_RightMouseKeyModifier_x003E_k__BackingField dataType="Enum" type="FrozenCore.Widgets.WidgetController+ModifierKeys" name="None" value="0" />
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">2626213635</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </item>
          <item dataType="Struct" type="Duality.Components.Diagnostics.ProfileRenderer" id="2893048527">
            <active dataType="Bool">false</active>
            <counterGraphs dataType="Struct" type="System.Collections.Generic.List`1[[System.String]]" id="4123267839">
              <_items dataType="Array" type="System.String[]" id="3954641198">
                <item dataType="String">Duality\Frame</item>
                <item dataType="String">Duality\Frame\Render</item>
                <item dataType="String">Duality\Frame\Update</item>
                <item dataType="String">Duality\Stats\Memory\TotalUsage</item>
              </_items>
              <_size dataType="Int">4</_size>
              <_version dataType="Int">4</_version>
            </counterGraphs>
            <drawGraphs dataType="Bool">true</drawGraphs>
            <gameobj dataType="ObjectRef">2626213635</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <keyToggleGraph dataType="Enum" type="OpenTK.Input.Key" name="F4" value="13" />
            <keyToggleTextPerf dataType="Enum" type="OpenTK.Input.Key" name="F2" value="11" />
            <keyToggleTextStat dataType="Enum" type="OpenTK.Input.Key" name="F3" value="12" />
            <textReportOptions dataType="Enum" type="Duality.ProfileReportOptions" name="LastValue" value="1" />
            <textReportPerf dataType="Bool">true</textReportPerf>
            <textReportStat dataType="Bool">true</textReportStat>
            <updateInterval dataType="Int">250</updateInterval>
          </item>
        </_items>
        <_size dataType="Int">4</_size>
        <_version dataType="Int">4</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2369798240" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Type[]" id="4164352923">
            <item dataType="Type" id="3780118422" value="Duality.Components.Transform" />
            <item dataType="Type" id="56258266" value="Duality.Components.Camera" />
            <item dataType="Type" id="2923306038" value="FrozenCore.Widgets.WidgetController" />
            <item dataType="Type" id="2251214330" value="Duality.Components.Diagnostics.ProfileRenderer" />
          </keys>
          <values dataType="Array" type="Duality.Component[]" id="1566245480">
            <item dataType="ObjectRef">691561271</item>
            <item dataType="ObjectRef">3163489442</item>
            <item dataType="ObjectRef">47164325</item>
            <item dataType="ObjectRef">2893048527</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">691561271</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="4135462993">gpcuL6aHnkCUpDRWylkPHw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Camera</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="3608675265">
      <active dataType="Bool">true</active>
      <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="3556018755">
        <_items dataType="Array" type="Duality.GameObject[]" id="3922714662">
          <item dataType="Struct" type="Duality.GameObject" id="3912879882">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2845244062">
              <_items dataType="Array" type="Duality.Component[]" id="1271784336" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="1978227518">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">3912879882</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <parentTransform dataType="Struct" type="Duality.Components.Transform" id="1674022901">
                    <active dataType="Bool">true</active>
                    <angle dataType="Float">0</angle>
                    <angleAbs dataType="Float">0</angleAbs>
                    <angleVel dataType="Float">0</angleVel>
                    <angleVelAbs dataType="Float">0</angleVelAbs>
                    <deriveAngle dataType="Bool">true</deriveAngle>
                    <gameobj dataType="ObjectRef">3608675265</gameobj>
                    <ignoreParent dataType="Bool">false</ignoreParent>
                    <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                    <parentTransform />
                    <pos dataType="Struct" type="OpenTK.Vector3">
                      <X dataType="Float">-300</X>
                      <Y dataType="Float">-200</Y>
                      <Z dataType="Float">0</Z>
                    </pos>
                    <posAbs dataType="Struct" type="OpenTK.Vector3">
                      <X dataType="Float">-300</X>
                      <Y dataType="Float">-200</Y>
                      <Z dataType="Float">0</Z>
                    </posAbs>
                    <scale dataType="Float">1</scale>
                    <scaleAbs dataType="Float">1</scaleAbs>
                    <vel dataType="Struct" type="OpenTK.Vector3">
                      <X dataType="Float">0</X>
                      <Y dataType="Float">0</Y>
                      <Z dataType="Float">0</Z>
                    </vel>
                    <velAbs dataType="Struct" type="OpenTK.Vector3">
                      <X dataType="Float">0</X>
                      <Y dataType="Float">0</Y>
                      <Z dataType="Float">0</Z>
                    </velAbs>
                  </parentTransform>
                  <pos dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">450</X>
                    <Y dataType="Float">300</Y>
                    <Z dataType="Float">-0.001</Z>
                  </pos>
                  <posAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">150</X>
                    <Y dataType="Float">100</Y>
                    <Z dataType="Float">-0.001</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </vel>
                  <velAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </velAbs>
                </item>
                <item dataType="Struct" type="FrozenCore.Widgets.SkinnedButton" id="2458700448">
                  <_activeArea dataType="Enum" type="FrozenCore.Widgets.ActiveArea" name="All" value="7" />
                  <_leftClickArgument />
                  <_next />
                  <_onLeftClick dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Script]]">
                    <contentPath dataType="String">Data\Scripts\ProgressIncrease.ProgressIncrease.res</contentPath>
                  </_onLeftClick>
                  <_onRightClick dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Script]]">
                    <contentPath />
                  </_onRightClick>
                  <_overrideAutoZ dataType="Bool">false</_overrideAutoZ>
                  <_previous />
                  <_rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">50</H>
                    <W dataType="Float">100</W>
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                  </_rect>
                  <_repeatLeftClickEvery dataType="Float">0.5</_repeatLeftClickEvery>
                  <_rightClickArgument />
                  <_skin dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Resources.Widgets.WidgetSkin]]">
                    <contentPath dataType="String">Data\Graphics\Skins\YellowButton.WidgetSkin.res</contentPath>
                  </_skin>
                  <_status dataType="Enum" type="FrozenCore.Widgets.Widget+WidgetStatus" name="Normal" value="0" />
                  <_text dataType="String">Progress++</_text>
                  <_textColor dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </_textColor>
                  <_textFont dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                    <contentPath dataType="String">Default:Font:GenericSansSerif12</contentPath>
                  </_textFont>
                  <_tint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </_tint>
                  <_visibleRect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">0</H>
                    <W dataType="Float">0</W>
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                  </_visibleRect>
                  <_visiblityFlag dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">3912879882</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
              <_version dataType="Int">2</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="375416202" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="1368115644">
                  <item dataType="ObjectRef">3780118422</item>
                  <item dataType="Type" id="2264193604" value="FrozenCore.Widgets.SkinnedButton" />
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="3524352662">
                  <item dataType="ObjectRef">1978227518</item>
                  <item dataType="ObjectRef">2458700448</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1978227518</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="504471400">eIv1jMzLzUSg4q40sulavw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">Button</name>
            <parent dataType="ObjectRef">3608675265</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="114325725">
            <active dataType="Bool">true</active>
            <children dataType="Struct" type="System.Collections.Generic.List`1[[Duality.GameObject]]" id="1878381901">
              <_items dataType="Array" type="Duality.GameObject[]" id="958276646" length="4">
                <item dataType="Struct" type="Duality.GameObject" id="4189188617">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1596241161">
                    <_items dataType="Array" type="Duality.Component[]" id="3123694222" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="2254536253">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">4189188617</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                        <parentTransform dataType="Struct" type="Duality.Components.Transform" id="2474640657">
                          <active dataType="Bool">true</active>
                          <angle dataType="Float">0</angle>
                          <angleAbs dataType="Float">0</angleAbs>
                          <angleVel dataType="Float">0</angleVel>
                          <angleVelAbs dataType="Float">0</angleVelAbs>
                          <deriveAngle dataType="Bool">true</deriveAngle>
                          <gameobj dataType="ObjectRef">114325725</gameobj>
                          <ignoreParent dataType="Bool">false</ignoreParent>
                          <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                          <parentTransform dataType="ObjectRef">1674022901</parentTransform>
                          <pos dataType="Struct" type="OpenTK.Vector3">
                            <X dataType="Float">50</X>
                            <Y dataType="Float">50</Y>
                            <Z dataType="Float">0</Z>
                          </pos>
                          <posAbs dataType="Struct" type="OpenTK.Vector3">
                            <X dataType="Float">-250</X>
                            <Y dataType="Float">-150</Y>
                            <Z dataType="Float">0</Z>
                          </posAbs>
                          <scale dataType="Float">1</scale>
                          <scaleAbs dataType="Float">1</scaleAbs>
                          <vel dataType="Struct" type="OpenTK.Vector3">
                            <X dataType="Float">0</X>
                            <Y dataType="Float">0</Y>
                            <Z dataType="Float">0</Z>
                          </vel>
                          <velAbs dataType="Struct" type="OpenTK.Vector3">
                            <X dataType="Float">0</X>
                            <Y dataType="Float">0</Y>
                            <Z dataType="Float">0</Z>
                          </velAbs>
                        </parentTransform>
                        <pos dataType="Struct" type="OpenTK.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">-20</Y>
                          <Z dataType="Float">-0.001</Z>
                        </pos>
                        <posAbs dataType="Struct" type="OpenTK.Vector3">
                          <X dataType="Float">-250</X>
                          <Y dataType="Float">-170</Y>
                          <Z dataType="Float">-0.001</Z>
                        </posAbs>
                        <scale dataType="Float">1</scale>
                        <scaleAbs dataType="Float">1</scaleAbs>
                        <vel dataType="Struct" type="OpenTK.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">0</Y>
                          <Z dataType="Float">0</Z>
                        </vel>
                        <velAbs dataType="Struct" type="OpenTK.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">0</Y>
                          <Z dataType="Float">0</Z>
                        </velAbs>
                      </item>
                      <item dataType="Struct" type="FrozenCore.Widgets.SkinnedRadioButton" id="336327436">
                        <_activeArea dataType="Enum" type="FrozenCore.Widgets.ActiveArea" name="LeftBorder" value="2" />
                        <_checkedArgument />
                        <_glyphSkin dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Resources.Widgets.WidgetSkin]]">
                          <contentPath dataType="String">Data\Graphics\Skins\RadioGlyph.WidgetSkin.res</contentPath>
                        </_glyphSkin>
                        <_isChecked dataType="Bool">true</_isChecked>
                        <_next />
                        <_onChecked dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Script]]">
                          <contentPath />
                        </_onChecked>
                        <_onUnchecked dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Script]]">
                          <contentPath />
                        </_onUnchecked>
                        <_overrideAutoZ dataType="Bool">false</_overrideAutoZ>
                        <_previous />
                        <_radioGroup dataType="String">RGroup</_radioGroup>
                        <_rect dataType="Struct" type="Duality.Rect">
                          <H dataType="Float">30</H>
                          <W dataType="Float">200</W>
                          <X dataType="Float">0</X>
                          <Y dataType="Float">0</Y>
                        </_rect>
                        <_skin dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Resources.Widgets.WidgetSkin]]">
                          <contentPath dataType="String">Data\Graphics\Skins\RadioButton.WidgetSkin.res</contentPath>
                        </_skin>
                        <_status dataType="Enum" type="FrozenCore.Widgets.Widget+WidgetStatus" name="Normal" value="0" />
                        <_text dataType="String">It's a Radio Button</_text>
                        <_textAlignment dataType="Enum" type="Duality.Alignment" name="Left" value="1" />
                        <_textColor dataType="Struct" type="Duality.Drawing.ColorRgba">
                          <A dataType="Byte">255</A>
                          <B dataType="Byte">255</B>
                          <G dataType="Byte">255</G>
                          <R dataType="Byte">255</R>
                        </_textColor>
                        <_textFont dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                          <contentPath dataType="String">Default:Font:GenericSansSerif12</contentPath>
                        </_textFont>
                        <_tint dataType="Struct" type="Duality.Drawing.ColorRgba">
                          <A dataType="Byte">255</A>
                          <B dataType="Byte">255</B>
                          <G dataType="Byte">255</G>
                          <R dataType="Byte">255</R>
                        </_tint>
                        <_uncheckedArgument />
                        <_visibleRect dataType="Struct" type="Duality.Rect">
                          <H dataType="Float">0</H>
                          <W dataType="Float">0</W>
                          <X dataType="Float">0</X>
                          <Y dataType="Float">0</Y>
                        </_visibleRect>
                        <_visiblityFlag dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">4189188617</gameobj>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1191065152" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="1966623171">
                        <item dataType="ObjectRef">3780118422</item>
                        <item dataType="Type" id="1897665574" value="FrozenCore.Widgets.SkinnedRadioButton" />
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="288958136">
                        <item dataType="ObjectRef">2254536253</item>
                        <item dataType="ObjectRef">336327436</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">2254536253</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="243775465">0LYm/5d+o0SUqHgNO5XKeg==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Radio1</name>
                  <parent dataType="ObjectRef">114325725</parent>
                  <prefabLink />
                </item>
                <item dataType="Struct" type="Duality.GameObject" id="1414517029">
                  <active dataType="Bool">true</active>
                  <children />
                  <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3808576213">
                    <_items dataType="Array" type="Duality.Component[]" id="2194208246" length="4">
                      <item dataType="Struct" type="Duality.Components.Transform" id="3774831961">
                        <active dataType="Bool">true</active>
                        <angle dataType="Float">0</angle>
                        <angleAbs dataType="Float">0</angleAbs>
                        <angleVel dataType="Float">0</angleVel>
                        <angleVelAbs dataType="Float">0</angleVelAbs>
                        <deriveAngle dataType="Bool">true</deriveAngle>
                        <gameobj dataType="ObjectRef">1414517029</gameobj>
                        <ignoreParent dataType="Bool">false</ignoreParent>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                        <parentTransform dataType="ObjectRef">2474640657</parentTransform>
                        <pos dataType="Struct" type="OpenTK.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">20</Y>
                          <Z dataType="Float">-0.001</Z>
                        </pos>
                        <posAbs dataType="Struct" type="OpenTK.Vector3">
                          <X dataType="Float">-250</X>
                          <Y dataType="Float">-130</Y>
                          <Z dataType="Float">-0.001</Z>
                        </posAbs>
                        <scale dataType="Float">1</scale>
                        <scaleAbs dataType="Float">1</scaleAbs>
                        <vel dataType="Struct" type="OpenTK.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">0</Y>
                          <Z dataType="Float">0</Z>
                        </vel>
                        <velAbs dataType="Struct" type="OpenTK.Vector3">
                          <X dataType="Float">0</X>
                          <Y dataType="Float">0</Y>
                          <Z dataType="Float">0</Z>
                        </velAbs>
                      </item>
                      <item dataType="Struct" type="FrozenCore.Widgets.SkinnedRadioButton" id="1856623144">
                        <_activeArea dataType="Enum" type="FrozenCore.Widgets.ActiveArea" name="LeftBorder" value="2" />
                        <_checkedArgument />
                        <_glyphSkin dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Resources.Widgets.WidgetSkin]]">
                          <contentPath dataType="String">Data\Graphics\Skins\RadioGlyph.WidgetSkin.res</contentPath>
                        </_glyphSkin>
                        <_isChecked dataType="Bool">false</_isChecked>
                        <_next />
                        <_onChecked dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Script]]">
                          <contentPath />
                        </_onChecked>
                        <_onUnchecked dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Script]]">
                          <contentPath />
                        </_onUnchecked>
                        <_overrideAutoZ dataType="Bool">false</_overrideAutoZ>
                        <_previous />
                        <_radioGroup dataType="String">RGroup</_radioGroup>
                        <_rect dataType="Struct" type="Duality.Rect">
                          <H dataType="Float">30</H>
                          <W dataType="Float">200</W>
                          <X dataType="Float">0</X>
                          <Y dataType="Float">0</Y>
                        </_rect>
                        <_skin dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Resources.Widgets.WidgetSkin]]">
                          <contentPath dataType="String">Data\Graphics\Skins\RadioButton.WidgetSkin.res</contentPath>
                        </_skin>
                        <_status dataType="Enum" type="FrozenCore.Widgets.Widget+WidgetStatus" name="Normal" value="0" />
                        <_text dataType="String">Also this one</_text>
                        <_textAlignment dataType="Enum" type="Duality.Alignment" name="Right" value="2" />
                        <_textColor dataType="Struct" type="Duality.Drawing.ColorRgba">
                          <A dataType="Byte">255</A>
                          <B dataType="Byte">255</B>
                          <G dataType="Byte">255</G>
                          <R dataType="Byte">255</R>
                        </_textColor>
                        <_textFont dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                          <contentPath dataType="String">Default:Font:GenericSansSerif12</contentPath>
                        </_textFont>
                        <_tint dataType="Struct" type="Duality.Drawing.ColorRgba">
                          <A dataType="Byte">255</A>
                          <B dataType="Byte">255</B>
                          <G dataType="Byte">255</G>
                          <R dataType="Byte">255</R>
                        </_tint>
                        <_uncheckedArgument />
                        <_visibleRect dataType="Struct" type="Duality.Rect">
                          <H dataType="Float">0</H>
                          <W dataType="Float">0</W>
                          <X dataType="Float">0</X>
                          <Y dataType="Float">0</Y>
                        </_visibleRect>
                        <_visiblityFlag dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                        <active dataType="Bool">true</active>
                        <gameobj dataType="ObjectRef">1414517029</gameobj>
                        <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                      </item>
                    </_items>
                    <_size dataType="Int">2</_size>
                    <_version dataType="Int">2</_version>
                  </compList>
                  <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1733373512" surrogate="true">
                    <header />
                    <body>
                      <keys dataType="Array" type="System.Type[]" id="1662315519">
                        <item dataType="ObjectRef">3780118422</item>
                        <item dataType="ObjectRef">1897665574</item>
                      </keys>
                      <values dataType="Array" type="Duality.Component[]" id="552242528">
                        <item dataType="ObjectRef">3774831961</item>
                        <item dataType="ObjectRef">1856623144</item>
                      </values>
                    </body>
                  </compMap>
                  <compTransform dataType="ObjectRef">3774831961</compTransform>
                  <identifier dataType="Struct" type="System.Guid" surrogate="true">
                    <header>
                      <data dataType="Array" type="System.Byte[]" id="3406174637">upaXeNjH0keuGeujSvBMTQ==</data>
                    </header>
                    <body />
                  </identifier>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <name dataType="String">Radio2</name>
                  <parent dataType="ObjectRef">114325725</parent>
                  <prefabLink />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
              <_version dataType="Int">4</_version>
            </children>
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="2075825848">
              <_items dataType="Array" type="Duality.Component[]" id="2703877927" length="4">
                <item dataType="ObjectRef">2474640657</item>
              </_items>
              <_size dataType="Int">1</_size>
              <_version dataType="Int">1</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2737978663" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="1778757268">
                  <item dataType="ObjectRef">3780118422</item>
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="1397315638">
                  <item dataType="ObjectRef">2474640657</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2474640657</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="927669040">WjvhrzFV3UO6Yvzw/rEkWg==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">RadioButtons</name>
            <parent dataType="ObjectRef">3608675265</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="3481137179">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="277084779">
              <_items dataType="Array" type="Duality.Component[]" id="2961155702" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="1546484815">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">3481137179</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <parentTransform dataType="ObjectRef">1674022901</parentTransform>
                  <pos dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">300</X>
                    <Y dataType="Float">30</Y>
                    <Z dataType="Float">-0.001</Z>
                  </pos>
                  <posAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">-170</Y>
                    <Z dataType="Float">-0.001</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </vel>
                  <velAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </velAbs>
                </item>
                <item dataType="Struct" type="FrozenCore.Widgets.SkinnedTextBlock" id="3873987385">
                  <_activeArea dataType="Enum" type="FrozenCore.Widgets.ActiveArea" name="None" value="0" />
                  <_next />
                  <_overrideAutoZ dataType="Bool">false</_overrideAutoZ>
                  <_previous />
                  <_rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">150</H>
                    <W dataType="Float">290</W>
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                  </_rect>
                  <_scrollbarButtonsSize dataType="Struct" type="OpenTK.Vector2">
                    <X dataType="Float">10</X>
                    <Y dataType="Float">20</Y>
                  </_scrollbarButtonsSize>
                  <_scrollbarCursorSize dataType="Struct" type="OpenTK.Vector2">
                    <X dataType="Float">10</X>
                    <Y dataType="Float">10</Y>
                  </_scrollbarCursorSize>
                  <_scrollbarCursorSkin dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Resources.Widgets.WidgetSkin]]">
                    <contentPath dataType="String">Data\Graphics\Skins\ScrollBarCursor.WidgetSkin.res</contentPath>
                  </_scrollbarCursorSkin>
                  <_scrollbarDecreaseButtonSkin dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Resources.Widgets.WidgetSkin]]">
                    <contentPath dataType="String">Data\Graphics\Skins\ScrollBarDecrease.WidgetSkin.res</contentPath>
                  </_scrollbarDecreaseButtonSkin>
                  <_scrollbarIncreaseButtonSkin dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Resources.Widgets.WidgetSkin]]">
                    <contentPath dataType="String">Data\Graphics\Skins\ScrollBarIncrease.WidgetSkin.res</contentPath>
                  </_scrollbarIncreaseButtonSkin>
                  <_scrollbarSkin dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Resources.Widgets.WidgetSkin]]">
                    <contentPath dataType="String">Data\Graphics\Skins\ScrollBar.WidgetSkin.res</contentPath>
                  </_scrollbarSkin>
                  <_scrollSpeed dataType="Int">5</_scrollSpeed>
                  <_skin dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Resources.Widgets.WidgetSkin]]">
                    <contentPath dataType="String">Data\Graphics\Skins\Panel.WidgetSkin.res</contentPath>
                  </_skin>
                  <_status dataType="Enum" type="FrozenCore.Widgets.Widget+WidgetStatus" name="Normal" value="0" />
                  <_text dataType="String">Testing/nTesting/nTesting/nTesting/nTesting/nTesting/nTesting/nTesting/nTesting/nTesting/nTesting/nTesting/nTesting...</_text>
                  <_textColor dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </_textColor>
                  <_textFont dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                    <contentPath />
                  </_textFont>
                  <_tint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </_tint>
                  <_visibleRect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">0</H>
                    <W dataType="Float">0</W>
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                  </_visibleRect>
                  <_visiblityFlag dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">3481137179</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
              <_version dataType="Int">2</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2765245640" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="554378945">
                  <item dataType="ObjectRef">3780118422</item>
                  <item dataType="Type" id="4094613166" value="FrozenCore.Widgets.SkinnedTextBlock" />
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="2310102752">
                  <item dataType="ObjectRef">1546484815</item>
                  <item dataType="ObjectRef">3873987385</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">1546484815</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="3727652371">OLTfPKIPbES01PwsHvDtTw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">TextBlock</name>
            <parent dataType="ObjectRef">3608675265</parent>
            <prefabLink />
          </item>
          <item dataType="Struct" type="Duality.GameObject" id="4101549498">
            <active dataType="Bool">true</active>
            <children />
            <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3681194190">
              <_items dataType="Array" type="Duality.Component[]" id="4207079376" length="4">
                <item dataType="Struct" type="Duality.Components.Transform" id="2166897134">
                  <active dataType="Bool">true</active>
                  <angle dataType="Float">0</angle>
                  <angleAbs dataType="Float">0</angleAbs>
                  <angleVel dataType="Float">0</angleVel>
                  <angleVelAbs dataType="Float">0</angleVelAbs>
                  <deriveAngle dataType="Bool">true</deriveAngle>
                  <gameobj dataType="ObjectRef">4101549498</gameobj>
                  <ignoreParent dataType="Bool">false</ignoreParent>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                  <parentTransform dataType="ObjectRef">1674022901</parentTransform>
                  <pos dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">50</X>
                    <Y dataType="Float">200</Y>
                    <Z dataType="Float">-0.001</Z>
                  </pos>
                  <posAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">-250</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">-0.001</Z>
                  </posAbs>
                  <scale dataType="Float">1</scale>
                  <scaleAbs dataType="Float">1</scaleAbs>
                  <vel dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </vel>
                  <velAbs dataType="Struct" type="OpenTK.Vector3">
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                    <Z dataType="Float">0</Z>
                  </velAbs>
                </item>
                <item dataType="Struct" type="FrozenCore.Widgets.SkinnedDropDownButton" id="378506863">
                  <_activeArea dataType="Enum" type="FrozenCore.Widgets.ActiveArea" name="RightBorder" value="4" />
                  <_dropDownHeight dataType="Int">100</_dropDownHeight>
                  <_dropdownSkin dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Resources.Widgets.WidgetSkin]]">
                    <contentPath dataType="String">Data\Graphics\Skins\Panel.WidgetSkin.res</contentPath>
                  </_dropdownSkin>
                  <_highlightSkin dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Resources.Widgets.WidgetSkin]]">
                    <contentPath dataType="String">Data\Graphics\Skins\Highlight.WidgetSkin.res</contentPath>
                  </_highlightSkin>
                  <_items dataType="Struct" type="System.Collections.Generic.List`1[[System.Object]]" id="3031111819">
                    <_items dataType="Array" type="System.Object[]" id="3150349942" length="4">
                      <item dataType="String">One</item>
                      <item dataType="String">Two</item>
                      <item dataType="Enum" type="System.StringSplitOptions" name="RemoveEmptyEntries" value="1" />
                    </_items>
                    <_size dataType="Int">3</_size>
                    <_version dataType="Int">15</_version>
                  </_items>
                  <_next />
                  <_overrideAutoZ dataType="Bool">false</_overrideAutoZ>
                  <_previous />
                  <_rect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">30</H>
                    <W dataType="Float">220</W>
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                  </_rect>
                  <_scrollbarButtonsSize dataType="Struct" type="OpenTK.Vector2">
                    <X dataType="Float">10</X>
                    <Y dataType="Float">20</Y>
                  </_scrollbarButtonsSize>
                  <_scrollbarCursorSize dataType="Struct" type="OpenTK.Vector2">
                    <X dataType="Float">10</X>
                    <Y dataType="Float">10</Y>
                  </_scrollbarCursorSize>
                  <_scrollbarCursorSkin dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Resources.Widgets.WidgetSkin]]">
                    <contentPath dataType="String">Data\Graphics\Skins\ScrollBarCursor.WidgetSkin.res</contentPath>
                  </_scrollbarCursorSkin>
                  <_scrollbarDecreaseButtonSkin dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Resources.Widgets.WidgetSkin]]">
                    <contentPath dataType="String">Data\Graphics\Skins\ScrollBarDecrease.WidgetSkin.res</contentPath>
                  </_scrollbarDecreaseButtonSkin>
                  <_scrollbarIncreaseButtonSkin dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Resources.Widgets.WidgetSkin]]">
                    <contentPath dataType="String">Data\Graphics\Skins\ScrollBarIncrease.WidgetSkin.res</contentPath>
                  </_scrollbarIncreaseButtonSkin>
                  <_scrollbarSkin dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Resources.Widgets.WidgetSkin]]">
                    <contentPath dataType="String">Data\Graphics\Skins\ScrollBar.WidgetSkin.res</contentPath>
                  </_scrollbarSkin>
                  <_scrollSpeed dataType="Int">5</_scrollSpeed>
                  <_skin dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Resources.Widgets.WidgetSkin]]">
                    <contentPath dataType="String">Data\Graphics\Skins\DropDown.WidgetSkin.res</contentPath>
                  </_skin>
                  <_status dataType="Enum" type="FrozenCore.Widgets.Widget+WidgetStatus" name="Normal" value="0" />
                  <_textColor dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </_textColor>
                  <_textFont dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
                    <contentPath />
                  </_textFont>
                  <_tint dataType="Struct" type="Duality.Drawing.ColorRgba">
                    <A dataType="Byte">255</A>
                    <B dataType="Byte">255</B>
                    <G dataType="Byte">255</G>
                    <R dataType="Byte">255</R>
                  </_tint>
                  <_visibleRect dataType="Struct" type="Duality.Rect">
                    <H dataType="Float">0</H>
                    <W dataType="Float">0</W>
                    <X dataType="Float">0</X>
                    <Y dataType="Float">0</Y>
                  </_visibleRect>
                  <_visiblityFlag dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
                  <active dataType="Bool">true</active>
                  <gameobj dataType="ObjectRef">4101549498</gameobj>
                  <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
                </item>
              </_items>
              <_size dataType="Int">2</_size>
              <_version dataType="Int">2</_version>
            </compList>
            <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3384876874" surrogate="true">
              <header />
              <body>
                <keys dataType="Array" type="System.Type[]" id="2683622796">
                  <item dataType="ObjectRef">3780118422</item>
                  <item dataType="Type" id="2786213796" value="FrozenCore.Widgets.SkinnedDropDownButton" />
                </keys>
                <values dataType="Array" type="Duality.Component[]" id="643790326">
                  <item dataType="ObjectRef">2166897134</item>
                  <item dataType="ObjectRef">378506863</item>
                </values>
              </body>
            </compMap>
            <compTransform dataType="ObjectRef">2166897134</compTransform>
            <identifier dataType="Struct" type="System.Guid" surrogate="true">
              <header>
                <data dataType="Array" type="System.Byte[]" id="27438872">3ufSWKrSX0qUSX75991FOw==</data>
              </header>
              <body />
            </identifier>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <name dataType="String">DropDown</name>
            <parent dataType="ObjectRef">3608675265</parent>
            <prefabLink />
          </item>
        </_items>
        <_size dataType="Int">4</_size>
        <_version dataType="Int">6</_version>
      </children>
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="976593592">
        <_items dataType="Array" type="Duality.Component[]" id="2243843369" length="4">
          <item dataType="ObjectRef">1674022901</item>
          <item dataType="Struct" type="FrozenCore.Widgets.SkinnedWindow" id="1395321343">
            <_activeArea dataType="Enum" type="FrozenCore.Widgets.ActiveArea" name="TopBorder" value="3" />
            <_buttonsSize dataType="Struct" type="OpenTK.Vector2">
              <X dataType="Float">20</X>
              <Y dataType="Float">13</Y>
            </_buttonsSize>
            <_canClose dataType="Bool">true</_canClose>
            <_canMaximize dataType="Bool">true</_canMaximize>
            <_canMinimize dataType="Bool">true</_canMinimize>
            <_closeButtonSize dataType="Struct" type="OpenTK.Vector2">
              <X dataType="Float">20</X>
              <Y dataType="Float">20</Y>
            </_closeButtonSize>
            <_closeButtonSkin dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Resources.Widgets.WidgetSkin]]">
              <contentPath dataType="String">Data\Graphics\Skins\WindowCloseBtn.WidgetSkin.res</contentPath>
            </_closeButtonSkin>
            <_isDraggable dataType="Bool">true</_isDraggable>
            <_maximizeButtonSkin dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Resources.Widgets.WidgetSkin]]">
              <contentPath dataType="String">Data\Graphics\Skins\WindowMaximizeBtn.WidgetSkin.res</contentPath>
            </_maximizeButtonSkin>
            <_maximizedSize dataType="Struct" type="OpenTK.Vector2">
              <X dataType="Float">800</X>
              <Y dataType="Float">600</Y>
            </_maximizedSize>
            <_minimizeButtonSkin dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Resources.Widgets.WidgetSkin]]">
              <contentPath dataType="String">Data\Graphics\Skins\WindowMinimizeBtn.WidgetSkin.res</contentPath>
            </_minimizeButtonSkin>
            <_next />
            <_overrideAutoZ dataType="Bool">false</_overrideAutoZ>
            <_previous />
            <_rect dataType="Struct" type="Duality.Rect">
              <H dataType="Float">400</H>
              <W dataType="Float">600</W>
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
            </_rect>
            <_restoreButtonSkin dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Resources.Widgets.WidgetSkin]]">
              <contentPath dataType="String">Data\Graphics\Skins\WindowRestoreBtn.WidgetSkin.res</contentPath>
            </_restoreButtonSkin>
            <_skin dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Resources.Widgets.WidgetSkin]]">
              <contentPath dataType="String">Data\Graphics\Skins\Window.WidgetSkin.res</contentPath>
            </_skin>
            <_status dataType="Enum" type="FrozenCore.Widgets.Widget+WidgetStatus" name="Normal" value="0" />
            <_tint dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">255</A>
              <B dataType="Byte">255</B>
              <G dataType="Byte">255</G>
              <R dataType="Byte">255</R>
            </_tint>
            <_title />
            <_titleColor dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">255</A>
              <B dataType="Byte">255</B>
              <G dataType="Byte">255</G>
              <R dataType="Byte">255</R>
            </_titleColor>
            <_titleFont dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
              <contentPath />
            </_titleFont>
            <_visibleRect dataType="Struct" type="Duality.Rect">
              <H dataType="Float">0</H>
              <W dataType="Float">0</W>
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
            </_visibleRect>
            <_visiblityFlag dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3608675265</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </item>
        </_items>
        <_size dataType="Int">2</_size>
        <_version dataType="Int">4</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3982696041" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Type[]" id="75134420">
            <item dataType="ObjectRef">3780118422</item>
            <item dataType="Type" id="2747348196" value="FrozenCore.Widgets.SkinnedWindow" />
          </keys>
          <values dataType="Array" type="Duality.Component[]" id="3779918774">
            <item dataType="ObjectRef">1674022901</item>
            <item dataType="ObjectRef">1395321343</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">1674022901</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1873780976">LksVFw25i0ir7HnqNscFZA==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Window</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="1045012679">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="1481896805">
        <_items dataType="Array" type="Duality.Component[]" id="1652600726" length="4">
          <item dataType="Struct" type="Duality.Components.Transform" id="3405327611">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <angleVel dataType="Float">0</angleVel>
            <angleVelAbs dataType="Float">0</angleVelAbs>
            <deriveAngle dataType="Bool">true</deriveAngle>
            <gameobj dataType="ObjectRef">1045012679</gameobj>
            <ignoreParent dataType="Bool">false</ignoreParent>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <parentTransform />
            <pos dataType="Struct" type="OpenTK.Vector3">
              <X dataType="Float">-100</X>
              <Y dataType="Float">250</Y>
              <Z dataType="Float">0</Z>
            </pos>
            <posAbs dataType="Struct" type="OpenTK.Vector3">
              <X dataType="Float">-100</X>
              <Y dataType="Float">250</Y>
              <Z dataType="Float">0</Z>
            </posAbs>
            <scale dataType="Float">1</scale>
            <scaleAbs dataType="Float">1</scaleAbs>
            <vel dataType="Struct" type="OpenTK.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">0</Z>
            </vel>
            <velAbs dataType="Struct" type="OpenTK.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">0</Z>
            </velAbs>
          </item>
          <item dataType="Struct" type="FrozenCore.Widgets.SkinnedProgressBar" id="3404892437">
            <_activeArea dataType="Enum" type="FrozenCore.Widgets.ActiveArea" name="None" value="0" />
            <_barSkin dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Resources.Widgets.WidgetSkin]]">
              <contentPath dataType="String">Data\Graphics\Skins\Progress.WidgetSkin.res</contentPath>
            </_barSkin>
            <_next />
            <_overrideAutoZ dataType="Bool">false</_overrideAutoZ>
            <_previous />
            <_rect dataType="Struct" type="Duality.Rect">
              <H dataType="Float">50</H>
              <W dataType="Float">200</W>
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
            </_rect>
            <_skin dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Resources.Widgets.WidgetSkin]]">
              <contentPath dataType="String">Data\Graphics\Skins\ProgressBar.WidgetSkin.res</contentPath>
            </_skin>
            <_status dataType="Enum" type="FrozenCore.Widgets.Widget+WidgetStatus" name="Normal" value="0" />
            <_text />
            <_textColor dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">255</A>
              <B dataType="Byte">255</B>
              <G dataType="Byte">255</G>
              <R dataType="Byte">255</R>
            </_textColor>
            <_textFont dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
              <contentPath />
            </_textFont>
            <_tint dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">255</A>
              <B dataType="Byte">255</B>
              <G dataType="Byte">255</G>
              <R dataType="Byte">255</R>
            </_tint>
            <_value dataType="Int">0</_value>
            <_visibleRect dataType="Struct" type="Duality.Rect">
              <H dataType="Float">0</H>
              <W dataType="Float">0</W>
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
            </_visibleRect>
            <_visiblityFlag dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1045012679</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </item>
        </_items>
        <_size dataType="Int">2</_size>
        <_version dataType="Int">2</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="3628943976" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Type[]" id="1747478543">
            <item dataType="ObjectRef">3780118422</item>
            <item dataType="Type" id="2484443054" value="FrozenCore.Widgets.SkinnedProgressBar" />
          </keys>
          <values dataType="Array" type="Duality.Component[]" id="3739116512">
            <item dataType="ObjectRef">3405327611</item>
            <item dataType="ObjectRef">3404892437</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">3405327611</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="2327840093">j+ZOAge7okaEP4fl1YPs6g==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">SkinnedProgressBar</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="1748443023">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="3040150253">
        <_items dataType="Array" type="Duality.Component[]" id="3943341798" length="4">
          <item dataType="Struct" type="Duality.Components.Transform" id="4108757955">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <angleVel dataType="Float">0</angleVel>
            <angleVelAbs dataType="Float">0</angleVelAbs>
            <deriveAngle dataType="Bool">true</deriveAngle>
            <gameobj dataType="ObjectRef">1748443023</gameobj>
            <ignoreParent dataType="Bool">false</ignoreParent>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <parentTransform />
            <pos dataType="Struct" type="OpenTK.Vector3">
              <X dataType="Float">-200</X>
              <Y dataType="Float">-300</Y>
              <Z dataType="Float">0</Z>
            </pos>
            <posAbs dataType="Struct" type="OpenTK.Vector3">
              <X dataType="Float">-200</X>
              <Y dataType="Float">-300</Y>
              <Z dataType="Float">0</Z>
            </posAbs>
            <scale dataType="Float">1</scale>
            <scaleAbs dataType="Float">1</scaleAbs>
            <vel dataType="Struct" type="OpenTK.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">0</Z>
            </vel>
            <velAbs dataType="Struct" type="OpenTK.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">0</Z>
            </velAbs>
          </item>
          <item dataType="Struct" type="FrozenCore.Widgets.SkinnedCommandGrid" id="3820687698">
            <_activeArea dataType="Enum" type="FrozenCore.Widgets.ActiveArea" name="None" value="0" />
            <_highlightSkin dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Resources.Widgets.WidgetSkin]]">
              <contentPath dataType="String">Data\Graphics\Skins\Highlight.WidgetSkin.res</contentPath>
            </_highlightSkin>
            <_itemPadding dataType="Struct" type="OpenTK.Vector4">
              <W dataType="Float">0</W>
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">0</Z>
            </_itemPadding>
            <_items dataType="Struct" type="System.Collections.Generic.List`1[[System.Object]]" id="1252799782">
              <_items dataType="Array" type="System.Object[]" id="2182712576" length="16">
                <item dataType="String">First Element</item>
                <item dataType="String">Second</item>
                <item dataType="Enum" type="System.DayOfWeek" name="Monday" value="1" />
                <item dataType="Enum" type="FrozenCore.Widgets.Widget+DirtyFlags" name="Skin, Custom1" value="10" />
                <item dataType="Struct" type="OpenTK.Vector2">
                  <X dataType="Float">5</X>
                  <Y dataType="Float">85</Y>
                </item>
              </_items>
              <_size dataType="Int">5</_size>
              <_version dataType="Int">53</_version>
            </_items>
            <_keyDown dataType="Enum" type="OpenTK.Input.Key" name="Down" value="46" />
            <_keyLeft dataType="Enum" type="OpenTK.Input.Key" name="Left" value="47" />
            <_keyRight dataType="Enum" type="OpenTK.Input.Key" name="Right" value="48" />
            <_keyUp dataType="Enum" type="OpenTK.Input.Key" name="Up" value="45" />
            <_leftClickArgument />
            <_next />
            <_onLeftClick dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Script]]">
              <contentPath />
            </_onLeftClick>
            <_onRightClick dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Script]]">
              <contentPath />
            </_onRightClick>
            <_overrideAutoZ dataType="Bool">false</_overrideAutoZ>
            <_previous />
            <_rect dataType="Struct" type="Duality.Rect">
              <H dataType="Float">80</H>
              <W dataType="Float">400</W>
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
            </_rect>
            <_rightClickArgument />
            <_scrollbarButtonsSize dataType="Struct" type="OpenTK.Vector2">
              <X dataType="Float">10</X>
              <Y dataType="Float">20</Y>
            </_scrollbarButtonsSize>
            <_scrollbarCursorSize dataType="Struct" type="OpenTK.Vector2">
              <X dataType="Float">10</X>
              <Y dataType="Float">10</Y>
            </_scrollbarCursorSize>
            <_scrollbarCursorSkin dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Resources.Widgets.WidgetSkin]]">
              <contentPath dataType="String">Data\Graphics\Skins\ScrollBarCursor.WidgetSkin.res</contentPath>
            </_scrollbarCursorSkin>
            <_scrollbarDecreaseButtonSkin dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Resources.Widgets.WidgetSkin]]">
              <contentPath dataType="String">Data\Graphics\Skins\ScrollBarDecrease.WidgetSkin.res</contentPath>
            </_scrollbarDecreaseButtonSkin>
            <_scrollbarIncreaseButtonSkin dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Resources.Widgets.WidgetSkin]]">
              <contentPath dataType="String">Data\Graphics\Skins\ScrollBarIncrease.WidgetSkin.res</contentPath>
            </_scrollbarIncreaseButtonSkin>
            <_scrollbarSkin dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Resources.Widgets.WidgetSkin]]">
              <contentPath dataType="String">Data\Graphics\Skins\ScrollBar.WidgetSkin.res</contentPath>
            </_scrollbarSkin>
            <_skin dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Resources.Widgets.WidgetSkin]]">
              <contentPath dataType="String">Data\Graphics\Skins\Panel.WidgetSkin.res</contentPath>
            </_skin>
            <_status dataType="Enum" type="FrozenCore.Widgets.Widget+WidgetStatus" name="Normal" value="0" />
            <_textColor dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">255</A>
              <B dataType="Byte">255</B>
              <G dataType="Byte">255</G>
              <R dataType="Byte">255</R>
            </_textColor>
            <_textFont dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
              <contentPath />
            </_textFont>
            <_tint dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">255</A>
              <B dataType="Byte">0</B>
              <G dataType="Byte">255</G>
              <R dataType="Byte">255</R>
            </_tint>
            <_visibleRect dataType="Struct" type="Duality.Rect">
              <H dataType="Float">0</H>
              <W dataType="Float">0</W>
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
            </_visibleRect>
            <_visiblityFlag dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">1748443023</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </item>
        </_items>
        <_size dataType="Int">2</_size>
        <_version dataType="Int">2</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="2490376440" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Type[]" id="1518136455">
            <item dataType="ObjectRef">3780118422</item>
            <item dataType="Type" id="2447628110" value="FrozenCore.Widgets.SkinnedCommandGrid" />
          </keys>
          <values dataType="Array" type="Duality.Component[]" id="873953664">
            <item dataType="ObjectRef">4108757955</item>
            <item dataType="ObjectRef">3820687698</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">4108757955</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="1161671557">ZFaHobCK4Emj0ENRTxgNCw==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">SkinnedCommandGrid</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="Struct" type="Duality.GameObject" id="3005610690">
      <active dataType="Bool">true</active>
      <children />
      <compList dataType="Struct" type="System.Collections.Generic.List`1[[Duality.Component]]" id="171494988">
        <_items dataType="Array" type="Duality.Component[]" id="2673313700" length="4">
          <item dataType="Struct" type="Duality.Components.Transform" id="1070958326">
            <active dataType="Bool">true</active>
            <angle dataType="Float">0</angle>
            <angleAbs dataType="Float">0</angleAbs>
            <angleVel dataType="Float">0</angleVel>
            <angleVelAbs dataType="Float">0</angleVelAbs>
            <deriveAngle dataType="Bool">true</deriveAngle>
            <gameobj dataType="ObjectRef">3005610690</gameobj>
            <ignoreParent dataType="Bool">false</ignoreParent>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
            <parentTransform />
            <pos dataType="Struct" type="OpenTK.Vector3">
              <X dataType="Float">220</X>
              <Y dataType="Float">-300</Y>
              <Z dataType="Float">-0.001</Z>
            </pos>
            <posAbs dataType="Struct" type="OpenTK.Vector3">
              <X dataType="Float">220</X>
              <Y dataType="Float">-300</Y>
              <Z dataType="Float">-0.001</Z>
            </posAbs>
            <scale dataType="Float">1</scale>
            <scaleAbs dataType="Float">1</scaleAbs>
            <vel dataType="Struct" type="OpenTK.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">0</Z>
            </vel>
            <velAbs dataType="Struct" type="OpenTK.Vector3">
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
              <Z dataType="Float">0</Z>
            </velAbs>
          </item>
          <item dataType="Struct" type="FrozenCore.Widgets.SkinnedButton" id="1551431256">
            <_activeArea dataType="Enum" type="FrozenCore.Widgets.ActiveArea" name="All" value="7" />
            <_leftClickArgument />
            <_next />
            <_onLeftClick dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Script]]">
              <contentPath dataType="String">Data\Scripts\FocusOnGrid.FocusOnGrid.res</contentPath>
            </_onLeftClick>
            <_onRightClick dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Script]]">
              <contentPath />
            </_onRightClick>
            <_overrideAutoZ dataType="Bool">false</_overrideAutoZ>
            <_previous />
            <_rect dataType="Struct" type="Duality.Rect">
              <H dataType="Float">30</H>
              <W dataType="Float">80</W>
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
            </_rect>
            <_repeatLeftClickEvery dataType="Float">0.5</_repeatLeftClickEvery>
            <_rightClickArgument />
            <_skin dataType="Struct" type="Duality.ContentRef`1[[FrozenCore.Resources.Widgets.WidgetSkin]]">
              <contentPath dataType="String">Data\Graphics\Skins\YellowButton.WidgetSkin.res</contentPath>
            </_skin>
            <_status dataType="Enum" type="FrozenCore.Widgets.Widget+WidgetStatus" name="Normal" value="0" />
            <_text dataType="String">Focus</_text>
            <_textColor dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">255</A>
              <B dataType="Byte">255</B>
              <G dataType="Byte">255</G>
              <R dataType="Byte">255</R>
            </_textColor>
            <_textFont dataType="Struct" type="Duality.ContentRef`1[[Duality.Resources.Font]]">
              <contentPath dataType="String">Default:Font:GenericSansSerif12</contentPath>
            </_textFont>
            <_tint dataType="Struct" type="Duality.Drawing.ColorRgba">
              <A dataType="Byte">255</A>
              <B dataType="Byte">255</B>
              <G dataType="Byte">255</G>
              <R dataType="Byte">255</R>
            </_tint>
            <_visibleRect dataType="Struct" type="Duality.Rect">
              <H dataType="Float">0</H>
              <W dataType="Float">0</W>
              <X dataType="Float">0</X>
              <Y dataType="Float">0</Y>
            </_visibleRect>
            <_visiblityFlag dataType="Enum" type="Duality.Drawing.VisibilityFlag" name="Group0" value="1" />
            <active dataType="Bool">true</active>
            <gameobj dataType="ObjectRef">3005610690</gameobj>
            <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
          </item>
        </_items>
        <_size dataType="Int">2</_size>
        <_version dataType="Int">2</_version>
      </compList>
      <compMap dataType="Struct" type="System.Collections.Generic.Dictionary`2[[System.Type],[Duality.Component]]" id="1380834806" surrogate="true">
        <header />
        <body>
          <keys dataType="Array" type="System.Type[]" id="1776496582">
            <item dataType="ObjectRef">3780118422</item>
            <item dataType="ObjectRef">2264193604</item>
          </keys>
          <values dataType="Array" type="Duality.Component[]" id="4250053306">
            <item dataType="ObjectRef">1070958326</item>
            <item dataType="ObjectRef">1551431256</item>
          </values>
        </body>
      </compMap>
      <compTransform dataType="ObjectRef">1070958326</compTransform>
      <identifier dataType="Struct" type="System.Guid" surrogate="true">
        <header>
          <data dataType="Array" type="System.Byte[]" id="688381126">27O8AJmob0m1OBt2o7A4dg==</data>
        </header>
        <body />
      </identifier>
      <initState dataType="Enum" type="Duality.InitState" name="Initialized" value="1" />
      <name dataType="String">Button</name>
      <parent />
      <prefabLink />
    </item>
    <item dataType="ObjectRef">3912879882</item>
    <item dataType="ObjectRef">114325725</item>
    <item dataType="ObjectRef">3481137179</item>
    <item dataType="ObjectRef">4101549498</item>
    <item dataType="ObjectRef">4189188617</item>
    <item dataType="ObjectRef">1414517029</item>
  </serializeObj>
  <sourcePath />
</root>
<!-- XmlFormatterBase Document Separator -->
