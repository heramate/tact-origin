using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace RACTTerminal
{
    public class Parser
    {
        public event ParserEventHandler OnParserEvent;

        private States m_State = States.Ground;
        private Char m_CurrentChar = '\0';
        private string m_CurrentSequence = "";

        private ArrayList m_ParamList = new System.Collections.ArrayList();
        private CharEvents m_CharEvents = new CharEvents();
        private StateChangeEvents m_StateChangeEvents = new StateChangeEvents();
        private Params m_CurrentParams = new Params();

        public Parser(){}

        public void Parsestring(string aInstring)
        {
            States tNextState = States.None;
            E_Actions tNextAction = E_Actions.None;
            E_Actions tStateExitAction = E_Actions.None;
            E_Actions tStateEntryAction = E_Actions.None;

            foreach (Char tChar in aInstring)
            {
                m_CurrentChar = tChar;

                m_CharEvents.GetStateEventAction(m_State, m_CurrentChar, ref tNextState, ref tNextAction);

                if (tNextState != States.None && tNextState != this.m_State)
                {
                    m_StateChangeEvents.GetStateChangeAction(this.m_State, Transitions.Exit, ref tStateExitAction);

                    if (tStateExitAction != E_Actions.None) DoAction(tStateExitAction);
                }

                if (tNextAction != E_Actions.None) DoAction(tNextAction);

                if (tNextState != States.None && tNextState != this.m_State)
                {
                    this.m_State = tNextState;

                    m_StateChangeEvents.GetStateChangeAction(this.m_State, Transitions.Entry, ref tStateExitAction);
                    
                    if (tStateEntryAction != E_Actions.None) DoAction(tStateEntryAction);
                }
            }
        }

        private void DoAction(E_Actions aNextAction)
        {
            switch (aNextAction)
            {
                case E_Actions.Dispatch:
                case E_Actions.Collect:
                    this.m_CurrentSequence += m_CurrentChar.ToString();
                    break;

                case E_Actions.NewCollect:
                    this.m_CurrentSequence = m_CurrentChar.ToString();
                    this.m_CurrentParams.Clear();
                    break;

                case E_Actions.Param:
                    this.m_CurrentParams.Add(m_CurrentChar);
                    break;

                default:
                    break;
            }

            switch (aNextAction)
            {
                case E_Actions.Dispatch:
                case E_Actions.Execute:
                case E_Actions.Put:
                case E_Actions.OscStart:
                case E_Actions.OscPut:
                case E_Actions.OscEnd:
                case E_Actions.Hook:
                case E_Actions.Unhook:
                case E_Actions.Print:

                    //                    System.Console.Write ("Sequence = {0}, Char = {1}, PrmCount = {2}, State = {3}, NextAction = {4}\n",
                    //                        this.CurSequence, this.CurChar.ToString (), this.CurParams.Count ().ToString (), 
                    //                        this.State.ToString (), NextAction.ToString ());

                    if(OnParserEvent != null) OnParserEvent(this, new ParserEventArgs(aNextAction, m_CurrentChar, m_CurrentSequence, m_CurrentParams));
                    break;

                default:
                    break;
            }


            switch (aNextAction)
            {
                case E_Actions.Dispatch:
                    this.m_CurrentSequence = "";
                    this.m_CurrentParams.Clear();
                    break;
                default:
                    break;
            }
        }

        private enum States
        {
            None = 0,
            Ground = 1,
            EscapeIntrmdt = 2,
            Escape = 3,
            CsiEntry = 4,
            CsiIgnore = 5,
            CsiParam = 6,
            CsiIntrmdt = 7,
            Oscstring = 8,
            SosPmApcstring = 9,
            DcsEntry = 10,
            DcsParam = 11,
            DcsIntrmdt = 12,
            DcsIgnore = 13,
            DcsPassthrough = 14,
            Anywhere = 16
        }

        private enum Transitions
        {
            None = 0,
            Entry = 1,
            Exit = 2
        }

        private struct uc_StateChangeInfo
        {
            public States State;
            public Transitions Transition;   
            public E_Actions NextAction;

            public uc_StateChangeInfo(
                States p1,
                Transitions p2,
                E_Actions p3)
            {
                this.State = p1;
                this.Transition = p2;
                this.NextAction = p3;
            }
        }

        /// <summary>
        /// State Change Event 클래스 입니다.
        /// </summary>
        private class StateChangeEvents
        {

            private uc_StateChangeInfo[] Elements = 
			{
				new uc_StateChangeInfo (States.Oscstring,      Transitions.Entry, E_Actions.OscStart),
				new uc_StateChangeInfo (States.Oscstring,      Transitions.Exit,  E_Actions.OscEnd),
				new uc_StateChangeInfo (States.DcsPassthrough, Transitions.Entry, E_Actions.Hook),
				new uc_StateChangeInfo (States.DcsPassthrough, Transitions.Exit,  E_Actions.Unhook)
			};

            public StateChangeEvents()
            {
            }

            public System.Boolean GetStateChangeAction(States aState,Transitions aTransition,ref E_Actions aNextAction)
            {
                uc_StateChangeInfo tElement;

                for (System.Int32 i = 0; i < Elements.Length; i++)
                {
                    tElement = Elements[i];

                    if (aState == tElement.State && aTransition == tElement.Transition)
                    {
                        aNextAction = tElement.NextAction;
                        return true;
                    }
                }

                return false;
            }
        }

        private struct CharEventInfo
        {
            public States CurState;
            public System.Char CharFrom;
            public System.Char CharTo;
            public E_Actions NextAction;
            public States NextState; 

            public CharEventInfo(States aStates, Char aCharFrom, Char aCharTo,E_Actions aNextActions,States aNextState)
            {
                this.CurState = aStates;
                this.CharFrom = aCharFrom;
                this.CharTo = aCharTo;
                this.NextAction = aNextActions;
                this.NextState = aNextState;
            }
        }

        private class CharEvents
        {
            public Boolean GetStateEventAction(States aCurState, Char aCurChar,ref States aNextState,ref E_Actions aNextAction)
            {
                CharEventInfo m_Element;

                if (aCurChar >= '\xA0' && aCurChar <= '\xFF')
                {
                    aCurChar -= '\x80';
                }

                for (System.Int32 i = 0; i < CharEvents.Elements.Length; i++)
                {
                    m_Element = CharEvents.Elements[i];

                    if (aCurChar >= m_Element.CharFrom &&
                        aCurChar <= m_Element.CharTo &&
                        (aCurState == m_Element.CurState || m_Element.CurState == States.Anywhere))
                    {
                        aNextState = m_Element.NextState;
                        aNextAction = m_Element.NextAction;
                        return true;
                    }
                }

                return false;
            }

            public CharEvents()
            {
            }

            public static CharEventInfo[] Elements = 
			{
				new CharEventInfo (States.Anywhere,      '\x1B', '\x1B', E_Actions.NewCollect, States.Escape),
				new CharEventInfo (States.Anywhere,      '\x18', '\x18', E_Actions.Execute,    States.Ground),
				new CharEventInfo (States.Anywhere,      '\x1A', '\x1A', E_Actions.Execute,    States.Ground),
				new CharEventInfo (States.Anywhere,      '\x1A', '\x1A', E_Actions.Execute,    States.Ground),
				new CharEventInfo (States.Anywhere,      '\x80', '\x8F', E_Actions.Execute,    States.Ground),
				new CharEventInfo (States.Anywhere,      '\x91', '\x97', E_Actions.Execute,    States.Ground),
				new CharEventInfo (States.Anywhere,      '\x99', '\x99', E_Actions.Execute,    States.Ground),
				new CharEventInfo (States.Anywhere,      '\x9A', '\x9A', E_Actions.Execute,    States.Ground),
				new CharEventInfo (States.Anywhere,      '\x9C', '\x9C', E_Actions.Execute,    States.Ground),
				new CharEventInfo (States.Anywhere,      '\x98', '\x98', E_Actions.None,       States.SosPmApcstring),
				new CharEventInfo (States.Anywhere,      '\x9E', '\x9F', E_Actions.None,       States.SosPmApcstring),
				new CharEventInfo (States.Anywhere,      '\x90', '\x90', E_Actions.NewCollect, States.DcsEntry),
				new CharEventInfo (States.Anywhere,      '\x9D', '\x9D', E_Actions.None,       States.Oscstring),
				new CharEventInfo (States.Anywhere,      '\x9B', '\x9B', E_Actions.NewCollect, States.CsiEntry),
				new CharEventInfo (States.Ground,        '\x00', '\x17', E_Actions.Execute,    States.None),
				new CharEventInfo (States.Ground,        '\x00', '\x17', E_Actions.Execute,    States.None),
				new CharEventInfo (States.Ground,        '\x19', '\x19', E_Actions.Execute,    States.None),
				new CharEventInfo (States.Ground,        '\x1C', '\x1F', E_Actions.Execute,    States.None),
				new CharEventInfo (States.Ground,        '\x20', '\x7F', E_Actions.Print,      States.None),
				new CharEventInfo (States.Ground,        '\x80', '\x8F', E_Actions.Execute,    States.None),
				new CharEventInfo (States.Ground,        '\x91', '\x9A', E_Actions.Execute,    States.None),
				new CharEventInfo (States.Ground,        '\x9C', '\x9C', E_Actions.Execute,    States.None),
				new CharEventInfo (States.EscapeIntrmdt, '\x00', '\x17', E_Actions.Execute,    States.None),
				new CharEventInfo (States.EscapeIntrmdt, '\x19', '\x19', E_Actions.Execute,    States.None),
				new CharEventInfo (States.EscapeIntrmdt, '\x1C', '\x1F', E_Actions.Execute,    States.None),
				new CharEventInfo (States.EscapeIntrmdt, '\x20', '\x2F', E_Actions.Collect,    States.None),
				new CharEventInfo (States.EscapeIntrmdt, '\x30', '\x7E', E_Actions.Dispatch,   States.Ground),
				new CharEventInfo (States.Escape,        '\x00', '\x17', E_Actions.Execute,    States.None),
				new CharEventInfo (States.Escape,        '\x19', '\x19', E_Actions.Execute,    States.Ground),
				new CharEventInfo (States.Escape,        '\x1C', '\x1F', E_Actions.Execute,    States.None),
				new CharEventInfo (States.Escape,        '\x58', '\x58', E_Actions.None,       States.SosPmApcstring),
				new CharEventInfo (States.Escape,        '\x5E', '\x5F', E_Actions.None,       States.SosPmApcstring),
				new CharEventInfo (States.Escape,        '\x50', '\x50', E_Actions.Collect,    States.DcsEntry),
				new CharEventInfo (States.Escape,        '\x5D', '\x5D', E_Actions.None,       States.Oscstring),
				new CharEventInfo (States.Escape,        '\x5B', '\x5B', E_Actions.Collect,    States.CsiEntry),
				new CharEventInfo (States.Escape,        '\x30', '\x4F', E_Actions.Dispatch,   States.Ground),
				new CharEventInfo (States.Escape,        '\x51', '\x57', E_Actions.Dispatch,   States.Ground),
				new CharEventInfo (States.Escape,        '\x59', '\x5A', E_Actions.Dispatch,   States.Ground),
				new CharEventInfo (States.Escape,        '\x5C', '\x5C', E_Actions.Dispatch,   States.Ground),
				new CharEventInfo (States.Escape,        '\x60', '\x7E', E_Actions.Dispatch,   States.Ground),
				new CharEventInfo (States.Escape,        '\x20', '\x2F', E_Actions.Collect,    States.EscapeIntrmdt),
				new CharEventInfo (States.CsiEntry,      '\x00', '\x17', E_Actions.Execute,    States.None),
				new CharEventInfo (States.CsiEntry,      '\x19', '\x19', E_Actions.Execute,    States.None),
				new CharEventInfo (States.CsiEntry,      '\x1C', '\x1F', E_Actions.Execute,    States.None),
				new CharEventInfo (States.CsiEntry,      '\x20', '\x2F', E_Actions.Collect,    States.CsiIntrmdt),
				new CharEventInfo (States.CsiEntry,      '\x3A', '\x3A', E_Actions.None,       States.CsiIgnore),
				new CharEventInfo (States.CsiEntry,      '\x3C', '\x3F', E_Actions.Collect,    States.CsiParam),
				new CharEventInfo (States.CsiEntry,      '\x3C', '\x3F', E_Actions.Collect,    States.CsiParam),
				new CharEventInfo (States.CsiEntry,      '\x30', '\x39', E_Actions.Param,      States.CsiParam),
				new CharEventInfo (States.CsiEntry,      '\x3B', '\x3B', E_Actions.Param,      States.CsiParam),
				new CharEventInfo (States.CsiEntry,      '\x3C', '\x3F', E_Actions.Collect,    States.CsiParam),
				new CharEventInfo (States.CsiEntry,      '\x40', '\x7E', E_Actions.Dispatch,   States.Ground),
				new CharEventInfo (States.CsiParam,      '\x00', '\x17', E_Actions.Execute,    States.None),
				new CharEventInfo (States.CsiParam,      '\x19', '\x19', E_Actions.Execute,    States.None),
				new CharEventInfo (States.CsiParam,      '\x1C', '\x1F', E_Actions.Execute,    States.None),
				new CharEventInfo (States.CsiParam,      '\x30', '\x39', E_Actions.Param,      States.None),
				new CharEventInfo (States.CsiParam,      '\x3B', '\x3B', E_Actions.Param,      States.None),
				new CharEventInfo (States.CsiParam,      '\x3A', '\x3A', E_Actions.None,       States.CsiIgnore),
				new CharEventInfo (States.CsiParam,      '\x3C', '\x3F', E_Actions.None,       States.CsiIgnore),
				new CharEventInfo (States.CsiParam,      '\x20', '\x2F', E_Actions.Collect,    States.CsiIntrmdt),
				new CharEventInfo (States.CsiParam,      '\x40', '\x7E', E_Actions.Dispatch,   States.Ground),
				new CharEventInfo (States.CsiIgnore,     '\x00', '\x17', E_Actions.Execute,    States.None),
				new CharEventInfo (States.CsiIgnore,     '\x19', '\x19', E_Actions.Execute,    States.None),
				new CharEventInfo (States.CsiIgnore,     '\x1C', '\x1F', E_Actions.Execute,    States.None),
				new CharEventInfo (States.CsiIgnore,     '\x40', '\x7E', E_Actions.None,       States.Ground),
				new CharEventInfo (States.CsiIntrmdt,    '\x00', '\x17', E_Actions.Execute,    States.None),
				new CharEventInfo (States.CsiIntrmdt,    '\x19', '\x19', E_Actions.Execute,    States.None),
				new CharEventInfo (States.CsiIntrmdt,    '\x1C', '\x1F', E_Actions.Execute,    States.None),
				new CharEventInfo (States.CsiIntrmdt,    '\x20', '\x2F', E_Actions.Collect,    States.None),
				new CharEventInfo (States.CsiIntrmdt,    '\x30', '\x3F', E_Actions.None,       States.CsiIgnore),
				new CharEventInfo (States.CsiIntrmdt,    '\x40', '\x7E', E_Actions.Dispatch,   States.Ground),
				new CharEventInfo (States.SosPmApcstring,'\x9C', '\x9C', E_Actions.None,       States.Ground),
				new CharEventInfo (States.DcsEntry,      '\x20', '\x2F', E_Actions.Collect,    States.DcsIntrmdt),
				new CharEventInfo (States.DcsEntry,      '\x3A', '\x3A', E_Actions.None,       States.DcsIgnore),
				new CharEventInfo (States.DcsEntry,      '\x30', '\x39', E_Actions.Param,      States.DcsParam),
				new CharEventInfo (States.DcsEntry,      '\x3B', '\x3B', E_Actions.Param,      States.DcsParam),
				new CharEventInfo (States.DcsEntry,      '\x3C', '\x3F', E_Actions.Collect,    States.DcsParam),
				new CharEventInfo (States.DcsEntry,      '\x40', '\x7E', E_Actions.None,       States.DcsPassthrough),
				new CharEventInfo (States.DcsIntrmdt,    '\x30', '\x3F', E_Actions.None,       States.DcsIgnore),
				new CharEventInfo (States.DcsIntrmdt,    '\x40', '\x7E', E_Actions.None,       States.DcsPassthrough),
				new CharEventInfo (States.DcsIgnore,     '\x9C', '\x9C', E_Actions.None,       States.Ground),
				new CharEventInfo (States.DcsParam,      '\x30', '\x39', E_Actions.Param,      States.None),
				new CharEventInfo (States.DcsParam,      '\x3B', '\x3B', E_Actions.Param,      States.None),
				new CharEventInfo (States.DcsParam,      '\x20', '\x2F', E_Actions.Collect,    States.DcsIntrmdt),
				new CharEventInfo (States.DcsParam,      '\x3A', '\x3A', E_Actions.None,       States.DcsIgnore),
				new CharEventInfo (States.DcsParam,      '\x3C', '\x3F', E_Actions.None,       States.DcsIgnore),
				new CharEventInfo (States.DcsPassthrough,'\x00', '\x17', E_Actions.Put,        States.None),
				new CharEventInfo (States.DcsPassthrough,'\x19', '\x19', E_Actions.Put,        States.None),
				new CharEventInfo (States.DcsPassthrough,'\x1C', '\x1F', E_Actions.Put,        States.None),
				new CharEventInfo (States.DcsPassthrough,'\x20', '\x7E', E_Actions.Put,        States.None),
				new CharEventInfo (States.DcsPassthrough,'\x9C', '\x9C', E_Actions.None,       States.Ground),
				new CharEventInfo (States.Oscstring,     '\x20', '\x7F', E_Actions.OscPut,     States.None),
				new CharEventInfo (States.Oscstring,     '\x9C', '\x9C', E_Actions.None,       States.Ground)
			};
        }
    } 
}
